using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer
{
    public class RabbitMqConsumer : IRabbitMqConsumer
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqConsumer> _logger;

        public RabbitMqConsumer(IModel channel, ILogger<RabbitMqConsumer> logger)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Certifique-se de usar a configuração correta da fila
            _channel.QueueDeclare(queue: "registrarContato",
                                  durable: true,  // Ajuste conforme necessário
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }



        public Object ConsumeMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Gerar um identificador único para esta operação
                    var uniqueId = $"{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}";

                    // Log do recebimento da mensagem com identificador único
                    _logger.LogInformation("Mensagem recebida [ID: {UniqueId}]: {Message}", uniqueId, message);

                    // Aqui você pode adicionar a lógica para processar a mensagem
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar a mensagem recebida.");
                }
            };

            try
            {
                _channel.BasicConsume(queue: "registrarContato",
                                     autoAck: true,
                                     consumer: consumer);
                _logger.LogInformation("Iniciando o consumo de mensagens da fila 'nome-da-fila'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao iniciar o consumo da fila 'nome-da-fila'.");
                throw; // Relançar a exceção para garantir que ela se propague
            }

            return null;
        }
    }
}
