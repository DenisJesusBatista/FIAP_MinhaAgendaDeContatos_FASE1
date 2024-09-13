using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MinhaAgendaDeContatos.Application;
using MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Infraestrutura;
using MinhaAgendaDeContatos.Infraestrutura.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;
using RabbitMQ.Client;
using System;
using MassTransit;
using MinhaAgendaDeContatos.Orchestrator.Api.BO;
using MinhaAgendaDeContatos.Orchestrator.Api.Controllers;

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
builder.Services.AddSwaggerGen();

// Configurar RabbitMQ
builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

// Configurar o factory e conexão RabbitMQ
builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var rabbitMqOptions = configuration.GetSection("RabbitMQ");

    return new ConnectionFactory
    {
        HostName = rabbitMqOptions["HostName"],
        Port = int.Parse(rabbitMqOptions["Port"]),
        UserName = rabbitMqOptions["UserName"],
        Password = rabbitMqOptions["Password"],
        VirtualHost = rabbitMqOptions["VirtualHost"]
    };
});

builder.Services.AddSingleton<IConnection>(sp =>
{
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

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguracao());
}).CreateMapper());


var configuration = builder.Configuration;
var servidor = configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
var usuario = configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
var senha = configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

builder.Services.AddMassTransit(x =>
 {
     x.UsingRabbitMq((context, cfg) =>
     {
         cfg.Host(servidor, "/", h =>
         {
             h.Username(usuario);
             h.Password(senha);
         });

         cfg.ReceiveEndpoint("registraContato", e =>
         {
             //e.Consumer<HomeController>();

         });

         cfg.ConfigureEndpoints(context);
     });
     x.AddConsumer<HomeController>();
 });

var app = builder.Build();

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
