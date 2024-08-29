using Microsoft.Extensions.Options;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using MinhaAgendaDeContatos.Produtor;
using RabbitMQ.Client;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Configuração para ler o appSettings.json
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Carregar as configurações do RabbitMQ do appsettings.json
                var rabbitMqSettings = context.Configuration.GetSection("RabbitMQ");
                services.Configure<RabbitMqSettings>(rabbitMqSettings);

                // Adicione o logger ao contêiner de serviços
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                });

                // Configure a conexão RabbitMQ e o canal
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
                    var connection = factory.CreateConnection();
                    return connection.CreateModel(); // Crie e retorne o canal
                });

                // Registre o RabbitMqProducer com base nas configurações
                services.AddSingleton<IRabbitMqProducer>(sp =>
                {
                    var channel = sp.GetRequiredService<IModel>();
                    var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                    var logger = sp.GetRequiredService<ILogger<RabbitMqProducer>>();
                    return new RabbitMqProducer(channel, logger); // Passar o canal, hostName e logger
                });

                // Registre o Worker
                services.AddHostedService<Worker>();
            });
}
