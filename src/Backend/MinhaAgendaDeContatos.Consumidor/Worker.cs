using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        #region ExecuteAsync antigo
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        //    using (var scope = _serviceScopeFactory.CreateScope())
        //    {
        //        var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
        //        var registrarContatoUseCase = scope.ServiceProvider.GetRequiredService<IRegistrarContatoUseCase>();

        //        using var channel = rabbitMqService.CreateChannel();

        //        // Declare all queues
        //        var queues = new[] { "registrarContato", "recuperarPorPrefixo", "recuperarPorId", "recuperarTodosContatos", "DelecaoContato", "AlteracaoContato" };
        //        foreach (var queue in queues)
        //        {
        //            channel.QueueDeclare(
        //                queue: queue,
        //                durable: false,
        //                exclusive: false,
        //                autoDelete: false,
        //                arguments: null
        //            );
        //        }

        //        var consumer = new EventingBasicConsumer(channel);

        //        consumer.Received += async (model, ea) =>
        //        {
        //            var body = ea.Body.ToArray();
        //            var message = Encoding.UTF8.GetString(body);

        //            _logger.LogInformation("Mensagem recebida: {message}", message);

        //            try
        //            {
        //                // Processar a mensagem conforme a fila
        //                var requisicao = JsonSerializer.Deserialize<RequisicaoRegistrarContatoJson>(message);
        //                if (requisicao != null)
        //                {
        //                    switch (ea.RoutingKey)
        //                    {
        //                        case "registrarContato":
        //                            await registrarContatoUseCase.Executar(requisicao);
        //                            break;
        //                            // Adicionar outros casos conforme necess�rio
        //                    }
        //                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        //                }
        //                else
        //                {
        //                    _logger.LogWarning("Mensagem recebida � nula ou inv�lida.");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Erro ao processar mensagem.");
        //                channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
        //            }
        //        };

        //        foreach (var queue in queues)
        //        {
        //            channel.BasicConsume(
        //                queue: queue,
        //                autoAck: false, // Use autoAck: false para reconhecimento manual
        //                consumer: consumer
        //            );
        //        }

        //        await Task.Delay(Timeout.Infinite, stoppingToken);
        //    }
        //}
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado �s: {time}", DateTimeOffset.Now);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var rabbitMqService = scope.ServiceProvider.GetRequiredService<IRabbitMqService>();
                var registrarContatoUseCase = scope.ServiceProvider.GetRequiredService<IRegistrarContatoUseCase>();

                using var channel = rabbitMqService.CreateChannel();

                // Declarar as filas com a configura��o correta
                var queues = new[] { "registrarContato", "recuperarPorPrefixo", "recuperarPorId", "recuperarTodosContatos", "DelecaoContato", "AlteracaoContato" };
                foreach (var queue in queues)
                {
                    channel.QueueDeclare(
                        queue: queue,
                        durable: true, // Ajuste conforme necess�rio
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
                        // Processar a mensagem conforme a fila
                        var requisicao = JsonSerializer.Deserialize<RootObject>(message);
                        if (requisicao != null)
                        {
                            switch (ea.RoutingKey)
                            {
                                case "registrarContato":

                                    var result = await registrarContatoUseCase.Executar(requisicao.Payload.Dados);

                                    var messageResult = new { Id = requisicao.Id, result };
                                    var queueName = "contatosRegistrados";

                                    await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                                    break;
                                    // Adicionar outros casos conforme necess�rio
                            }
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        else
                        {
                            _logger.LogWarning("Mensagem recebida � nula ou inv�lida.");
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
                _logger.LogInformation("Aguardando 5 segundos antes de continuar a execu��o.");

                // Atraso de 5 segundos
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                // Mensagem ap�s o atraso
                _logger.LogInformation("Atraso conclu�do. Continuando a execu��o.");

                // Continue a execu��o do servi�o conforme necess�rio
                await Task.Delay(Timeout.Infinite, stoppingToken); // Mant�m o servi�o em execu��o indefinidamente
            }
        }


    }
}
