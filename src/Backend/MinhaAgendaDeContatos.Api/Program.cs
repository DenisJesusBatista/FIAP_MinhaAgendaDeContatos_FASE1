using Microsoft.OpenApi.Models;
using MinhaAgendaDeContatos.Api.Filtros;
using MinhaAgendaDeContatos.Application;
using MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Infraestrutura;
using MinhaAgendaDeContatos.Infraestrutura.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AtualizarBaseDeDados();

app.Run();

void AtualizarBaseDeDados()
{
    var conexao = builder.Configuration.GetConexao();
    var nomeDatabase = builder.Configuration.GetNomeDataBase();

    // Verifica se o banco de dados existe
    bool bancoExiste = Database.VerificarExistenciaDatabase(conexao, nomeDatabase);

    if (bancoExiste == true)
    {
        app.MigrateBancoDados();
    }
    else
    {
        Database.CriarDatabase(conexao, nomeDatabase);
        app.MigrateBancoDados();
    }

        
}

public partial class Program { }