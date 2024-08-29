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
            _channel = channel;
            _logger = logger;
        }

        public void PublishMessage<T>(string queueName, T message)
        {
            // Declare the queue inside the method to ensure it exists
            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            try
            {
                var uniqueId = $"{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}";
                var jsonMessage = JsonSerializer.Serialize(new { Id = uniqueId, Payload = message });
                var body = Encoding.UTF8.GetBytes(jsonMessage);

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
                throw; // Re-throw the exception to ensure it bubbles up
            }
        }
    }
}