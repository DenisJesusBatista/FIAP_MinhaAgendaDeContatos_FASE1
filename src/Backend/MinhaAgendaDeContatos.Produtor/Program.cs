using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using MinhaAgendaDeContatos.Produtor;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Load RabbitMQ settings from appsettings.json
                services.Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMQ"));

                // Register RabbitMQ connection and channel
                services.AddSingleton<IModel>(sp =>
                {
                    var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                    var factory = new ConnectionFactory
                    {
                        HostName = settings.HostName,
                        Port = settings.Port,
                        UserName = settings.UserName,
                        Password = settings.Password,
                        VirtualHost = settings.VirtualHost
                    };

                    try
                    {
                        var connection = factory.CreateConnection();
                        return connection.CreateModel(); // Create and return the channel (IModel)
                    }
                    catch (Exception ex)
                    {
                        // Log error and rethrow if necessary
                        var logger = sp.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Failed to create RabbitMQ channel");
                        throw;
                    }
                    
                });

                // Register RabbitMqProducer
                services.AddSingleton<IRabbitMqProducer>(sp =>
                {
                    var channel = sp.GetRequiredService<IModel>();
                    var logger = sp.GetRequiredService<ILogger<RabbitMqProducer>>();
                    return new RabbitMqProducer(channel, logger);
                });

                // Register other services
                services.AddHostedService<Worker>();
            });
}
