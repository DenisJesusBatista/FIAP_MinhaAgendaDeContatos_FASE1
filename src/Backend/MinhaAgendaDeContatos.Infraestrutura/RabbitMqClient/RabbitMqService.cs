using Microsoft.Extensions.Configuration;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using RabbitMQ.Client;
using System;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConnection _connection;

    // Construtor sem o ILogger
    public RabbitMqService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
            // Outras configurações, se necessário
        };

        _connection = factory.CreateConnection();
    }

    public IModel CreateChannel()
    {
        return _connection.CreateModel();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
