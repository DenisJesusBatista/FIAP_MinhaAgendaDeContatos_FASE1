using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MinhaAgendaDeContatos.Produtor.RabbitMqProducer
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqProducer> _logger;

        public RabbitMqProducer(IModel channel, ILogger<RabbitMqProducer> logger)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishMessageAsync<T>(string queueName, T message)
        {
            // Declare a fila dentro do método para garantir que ela exista
            _channel.QueueDeclare(
                queue: queueName,
                durable: true, // Considere definir como true se precisar da durabilidade da fila
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            try
            {
                var uniqueId = $"{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}";
                var jsonMessage = JsonSerializer.Serialize(new { Id = uniqueId, Payload = message });
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                // Publicar a mensagem
                _channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body
                );

                _logger.LogInformation("Mensagem publicada com sucesso na fila {QueueName}: {Message}", queueName, jsonMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao publicar a mensagem na fila {QueueName}", queueName);
                throw; // Relançar a exceção para garantir que ela se propague
            }
        }
    }
}
