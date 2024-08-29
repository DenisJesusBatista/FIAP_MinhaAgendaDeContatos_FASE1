using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace MinhaAgendaDeContatos.Produtor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "delecaoQueue";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var jsonMessage = JsonSerializer.Deserialize<dynamic>(message);

                    _logger.LogInformation($"Received message: {jsonMessage}");
                    // Process message here
                };

                channel.BasicConsume(queue: _queueName,
                    autoAck: true,
                    consumer: consumer);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
