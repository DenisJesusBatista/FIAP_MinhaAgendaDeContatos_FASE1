//using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;
//using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
//using RabbitMQ.Client;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.




//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Configurar RabbitMQ
////builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
////builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

//// Configurar RabbitMQ
//builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
//builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

//// Configurar o factory e conexão RabbitMQ
//builder.Services.AddSingleton<IConnectionFactory>(sp =>
//{
//    return new ConnectionFactory
//    {
//        HostName = "localhost", // Configurações do RabbitMQ
//        UserName = "guest",
//        Password = "guest"
//    };
//});
//builder.Services.AddSingleton<IConnection>(sp =>
//{
//    var factory = sp.GetRequiredService<IConnectionFactory>();
//    return factory.CreateConnection();
//});
//builder.Services.AddSingleton<IModel>(sp =>
//{
//    var connection = sp.GetRequiredService<IConnection>();
//    return connection.CreateModel();
//});

//var app = builder.Build();



//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using MinhaAgendaDeContatos.Api.Filtros;
using MinhaAgendaDeContatos.Application;
using MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Infraestrutura;
using MinhaAgendaDeContatos.Infraestrutura.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using RabbitMQ.Client;
using System.Reflection;
using System;
using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;

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
builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

// Configurar o factory e conexão RabbitMQ
builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    return new ConnectionFactory
    {
        //HostName = "localhost", // Configurações do RabbitMQ
        HostName = "172.19.0.4", // Configurações do RabbitMQ
        Port = 5672,
        UserName = "guest",
        Password = "guest",
        VirtualHost = "/"

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
