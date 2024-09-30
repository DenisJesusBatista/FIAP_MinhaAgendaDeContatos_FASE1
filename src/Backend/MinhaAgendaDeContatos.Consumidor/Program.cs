using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Consumidor;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MinhaAgendaDeContatos.Domain.Extension;
using System.Threading.Tasks;
using AutoMapper;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;

class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        // Testar a conexão com o banco de dados
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
            //var testeConexao = new TesteConexao(context);
            //await testeConexao.TestarConexao();
        }

        // Executar o host
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureServices((context, services) =>
             {
                 var configuration = context.Configuration;

                 // Registrar o contexto do banco de dados
                 services.AddDbContext<MinhaAgendaDeContatosContext>(options =>
                     options.UseNpgsql(configuration.GetConnectionString("Conexao")));

                 // Registrar o FluentMigrator
                 services.AddFluentMigratorCore()
                     .ConfigureRunner(c => c
                         .AddPostgres()
                         .WithGlobalConnectionString(configuration.GetConnectionString("Conexao"))
                         .ScanIn(Assembly.Load("MinhaAgendaDeContatos.Infraestrutura")).For.All());

                 // Registrar as configurações do RabbitMQ
                 services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));

                 // Registrar serviços do RabbitMQ
                 services.AddSingleton<IRabbitMqService, RabbitMqService>();

                 // Registrar o canal RabbitMQ (IModel) após configurar o RabbitMQSettings
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

                 // Registrar o RabbitMqConsumer
                 services.AddSingleton<IRabbitMqConsumer>(sp =>
                 {
                     var channel = sp.GetRequiredService<IModel>();
                     var logger = sp.GetRequiredService<ILogger<RabbitMqConsumer>>();
                     return new RabbitMqConsumer(channel, logger);
                 });

                 // Registrar outros serviços
                 services.AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>();
                 services.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>();
                 services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
                 services.AddScoped<IRegistrarContatoUseCase, RegistrarContatoUseCase>();
                 services.AddScoped<IRecuperarPorIdUseCase, RecuperarPorIdUseCase>();
                 services.AddScoped<IDeletarContatoUseCase, DeletarContatoUseCase>();
                 services.AddScoped<IRecuperarTodosContatosUseCase, RecuperarTodosContatosUseCase>();
                 services.AddScoped<IUpdateContatoUseCase, UpdateContatoUseCase>();
                 services.AddScoped<IRecuperarPorPrefixoUseCase, RecuperarPorPrefixoUseCase>();

                 services.AddHostedService<Worker>();

                 // Configurar AutoMapper
                 services.AddAutoMapper(typeof(AutoMapperProfile));
             });
}
