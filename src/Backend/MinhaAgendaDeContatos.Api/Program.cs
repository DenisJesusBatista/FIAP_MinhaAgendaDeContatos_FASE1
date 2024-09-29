using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Api.Filtros;
using MinhaAgendaDeContatos.Application;
using MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Infraestrutura;
using MinhaAgendaDeContatos.Infraestrutura.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using Prometheus;
using RabbitMQ.Client;
using System.Reflection;
using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Options;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;
using MinhaAgendaDeContatos.Consumidor;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;

var builder = WebApplication.CreateBuilder(args);

// Configurar os serviços e o logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Adicionar logging para console
builder.Logging.AddDebug(); // Adicionar logging para debug
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));
builder.Logging.AddFilter("RabbitMQ.Client", LogLevel.Debug); // Adicionar logging detalhado para RabbitMQ

// Configurar rotas
builder.Services.AddRouting(option => option.LowercaseUrls = true);

// Configurar controllers
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    string version = "1.0";
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha agenda de contato", Version = version });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configurar RabbitMQ
builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddHostedService<MinhaAgendaDeContatos.Consumidor.Worker>();



// Registrar as configurações do RabbitMQ
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

// Registrar serviços do RabbitMQ
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

// Registrar o canal RabbitMQ (IModel) após configurar o RabbitMQSettings
builder.Services.AddSingleton<IModel>(sp =>
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
builder.Services.AddSingleton<IRabbitMqProducer>(sp =>
{
    var channel = sp.GetRequiredService<IModel>();
    var logger = sp.GetRequiredService<ILogger<RabbitMqProducer>>();
    return new RabbitMqProducer(channel, logger);
});

// Registrar o RabbitMqConsumer
builder.Services.AddSingleton<IRabbitMqConsumer>(sp =>
{
    var channel = sp.GetRequiredService<IModel>();
    var logger = sp.GetRequiredService<ILogger<RabbitMqConsumer>>();
    return new RabbitMqConsumer(channel, logger);
});

// Registrar outros serviços
builder.Services.AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>();
builder.Services.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>();
builder.Services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
builder.Services.AddScoped<IRegistrarContatoUseCase, RegistrarContatoUseCase>();
builder.Services.AddScoped<IRecuperarPorIdUseCase, RecuperarPorIdUseCase>();
builder.Services.AddScoped<IDeletarContatoUseCase, DeletarContatoUseCase>();
builder.Services.AddScoped<IRecuperarTodosContatosUseCase, RecuperarTodosContatosUseCase>();
builder.Services.AddScoped<IUpdateContatoUseCase, UpdateContatoUseCase>();
builder.Services.AddScoped<IRecuperarPorPrefixoUseCase, RecuperarPorPrefixoUseCase>();

builder.Services.AddHostedService<Worker>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configurar o factory e conexão RabbitMQ
builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    return new ConnectionFactory
    {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest"   
    };
});
builder.Services.AddSingleton<IConnection>(sp =>
{
    Thread.Sleep(20000);
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

// Configurar outros serviços e AutoMapper
builder.Services.AddRepositorio(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)))
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguracao());
}).CreateMapper());

// Adicionar métricas do Prometheus
builder.Services.UseHttpClientMetrics();

var app = builder.Build();

// Adicionar métricas do Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Configurar e atualizar o banco de dados
DatabaseSetup.AtualizarBaseDeDados(builder.Configuration, app);

app.Run();

public partial class Program { }
