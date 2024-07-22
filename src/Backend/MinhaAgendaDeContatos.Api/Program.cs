using Microsoft.OpenApi.Models;
using MinhaAgendaDeContatos.Api.Filtros;
using MinhaAgendaDeContatos.Application;
using MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Infraestrutura;
using MinhaAgendaDeContatos.Infraestrutura.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
using Prometheus;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Deixar todas as URLs com letra minusculas.
builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Registar a documentação no Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Defina a versão como uma string
    string version = "1.0";

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha agenda de contato", Version = version });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddRepositorio(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);


//Registrar o filtro para qualquer excessao chamar a classe
builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)))
    .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

//Configurando AutoMapper na injeção de dependência
builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfiguracao());
}).CreateMapper());

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

//Adicona as métricas ao código para gerar métricas do Prometheus
builder.Services.UseHttpClientMetrics();

var app = builder.Build();

//Adicona as métricas ao código para gerar métricas do Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//DatabaseSetup.AtualizarBaseDeDados(builder.Configuration, app);

app.Run();

public partial class Program { }