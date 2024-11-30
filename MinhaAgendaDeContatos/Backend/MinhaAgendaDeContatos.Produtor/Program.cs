using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using MinhaAgendaDeContatos.Produtor;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Carregar as configurações do RabbitMQ do appsettings.json
                services.Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMQ"));

                // Registrar a conexão e o canal do RabbitMQ
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
                        return connection.CreateModel(); // Criar e retornar o canal (IModel)
                    }
                    catch (Exception ex)
                    {
                        // Registrar o erro e relançar se necessário
                        var logger = sp.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Falha ao criar o canal RabbitMQ");
                        throw;
                    }

                });

                // Registrar o RabbitMqProducer
                services.AddSingleton<IRabbitMqProducer>(sp =>
                {
                    var channel = sp.GetRequiredService<IModel>();
                    var logger = sp.GetRequiredService<ILogger<RabbitMqProducer>>();
                    return new RabbitMqProducer(channel, logger);
                });

                // Registrar um serviço de fundo ( execução de segundo plano ) que será executado continuamente ou periodicamente
                services.AddHostedService<Worker>();

            });
}
