using Microsoft.Extensions.Options;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure RabbitMQ settings and services
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

// Register RabbitMQ connection and channel
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
    var connection = factory.CreateConnection();
    return connection.CreateModel();
});

// Register RabbitMqProducer
builder.Services.AddSingleton<IRabbitMqProducer>(sp =>
{
    var channel = sp.GetRequiredService<IModel>();
    var settings = sp.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
    var logger = sp.GetRequiredService<ILogger<RabbitMqProducer>>();
    return new RabbitMqProducer(channel, logger);
});

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

app.Run();
