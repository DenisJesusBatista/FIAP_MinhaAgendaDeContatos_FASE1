using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Consumidor
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<Worker> _logger;

        private readonly IRabbitMqProducer _rabbitMqProducer;

        public Worker(IRabbitMqProducer rabbitMqProducer, IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger)
        {
            _rabbitMqProducer = rabbitMqProducer;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado às: {time}", DateTimeOffset.Now);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
                var registrarContatoUseCase = scope.ServiceProvider.GetRequiredService<IRegistrarContatoUseCase>();
                var recuperarPorIdUseCase = scope.ServiceProvider.GetRequiredService<IRecuperarPorIdUseCase>();                

                using var channel = rabbitMqService.CreateChannel();

                // Declarar as filas com a configuração correta
                var queues = new[] { "registrarContato", "recuperarPorPrefixo", "recuperarPorId", "recuperarTodosContatos", "DelecaoContato", "AlteracaoContato" };
                foreach (var queue in queues)
                {
                    channel.QueueDeclare(
                        queue: queue,
                        durable: true, // Ajuste conforme necessário
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                }

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation("Mensagem recebida: {message}", message);

                    try
                    {

                        //REGEX = MESSAGE

                        // Processar a mensagem conforme a fila
                        var requisicao = JsonSerializer.Deserialize<RootObject>(message);

                        if (requisicao != null)
                        {
                            switch (ea.RoutingKey)
                            {
                                case "registrarContato":

                                    var resultRegistrar = await registrarContatoUseCase.Executar(requisicao.Payload.Dados);
                                    var messageResult = new { Id = requisicao.Id, resultRegistrar };

                                    await _rabbitMqProducer.PublishMessageAsync("contatosRegistrados", message);

                                    break;

                                case "recuperarPorId":

                                   var resultRecuperarId = await recuperarPorIdUseCase.Executar(requisicao.Payload.Dados.Id);

                                   await _rabbitMqProducer.PublishMessageAsync("contatoPorId", message);

                                    break;
                            }
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        else
                        {
                            _logger.LogWarning("Mensagem recebida é nula ou inválida.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro ao processar mensagem.");
                        channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                foreach (var queue in queues)
                {
                    channel.BasicConsume(
                        queue: queue,
                        autoAck: false, // Use autoAck: false para reconhecimento manual
                        consumer: consumer
                    );
                }

                // Mensagem antes do atraso
                _logger.LogInformation("Aguardando 5 segundos antes de continuar a execução.");

                // Atraso de 5 segundos
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                // Mensagem após o atraso
                _logger.LogInformation("Atraso concluído. Continuando a execução.");

                // Continue a execução do serviço conforme necessário
                await Task.Delay(Timeout.Infinite, stoppingToken); // Mantém o serviço em execução indefinidamente
            }
        }


    }
}
