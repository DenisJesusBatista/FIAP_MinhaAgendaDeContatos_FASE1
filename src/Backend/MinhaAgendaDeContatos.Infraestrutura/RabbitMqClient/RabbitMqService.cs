using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using RabbitMQ.Client;
using System;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "host.docker.internal", // Configurações do RabbitMQ
            UserName = "guest",
            Password = "guest",
            Port = 5672
            // Outras configurações, se necessário
        };

        _connection = factory.CreateConnection();
    }

    //public RabbitMqService(IConfiguration configuration, ILogger<RabbitMqService> logger)
    //{
    //    _logger = logger;

    //    var rabbitMqConfig = configuration.GetSection("RabbitMQ").Get<RabbitMqSettings>();

    //    var factory = new ConnectionFactory
    //    {
    //        HostName = rabbitMqConfig.HostName,
    //        Port = rabbitMqConfig.Port,
    //        UserName = rabbitMqConfig.UserName,
    //        Password = rabbitMqConfig.Password,
    //        VirtualHost = rabbitMqConfig.VirtualHost
    //    };

    //    try
    //    {
    //        _connection = factory.CreateConnection();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Falha ao criar a conexão com RabbitMQ");
    //        throw;
    //    }
    //}

    public IModel CreateChannel()
    {
        return _connection.CreateModel();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
