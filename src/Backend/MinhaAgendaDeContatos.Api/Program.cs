using Microsoft.OpenApi.Models;
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

var builder = WebApplication.CreateBuilder(args);

// Deixar todas as URLs com letra min�sculas.
builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddControllers();

// Saiba mais sobre como configurar Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Registrar a documenta��o no Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Defina a vers�o como uma string
    string version = "1.0";

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha agenda de contato", Version = version });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configurar o RabbitMQ
builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();

builder.Services.AddRepositorio(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);

// Registrar o filtro para exce��es
builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)))
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

// Configurando AutoMapper na inje��o de depend�ncia
builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguracao());
}).CreateMapper());

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

// Adicionar as m�tricas ao c�digo para gerar m�tricas do Prometheus
builder.Services.UseHttpClientMetrics();

// Registrar a f�brica de conex�o e os canais RabbitMQ
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
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

var app = builder.Build();

// Adicionar as m�tricas ao c�digo para gerar m�tricas do Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

// Configurar o pipeline de requisi��es HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DatabaseSetup.AtualizarBaseDeDados(builder.Configuration, app);

app.Run();

public partial class Program { }
