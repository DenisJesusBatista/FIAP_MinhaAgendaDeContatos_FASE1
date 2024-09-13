using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Orchestrator.Api.BO;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;

namespace MinhaAgendaDeContatos.Orchestrator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase, IConsumer<RegistrarContatoBO>
    {        
        private readonly IRabbitMqProducer _rabbitMqProducer;        
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public HomeController(IRabbitMqProducer rabbitMqProducer, IBus bus, IConfiguration configuration)
        {
            _rabbitMqProducer = rabbitMqProducer;
            _configuration = configuration;
            _bus = bus;
        }

        [HttpGet(Name = "Start")]
        public async Task<IActionResult> Get()
        {
            var nomeFila = _configuration
                .GetSection("MassTransit")["NomeFila"] ?? string.Empty;


            var endpoint = await _bus
                .GetSendEndpoint(new Uri($"queue:{nomeFila}"));


            return Ok("Service is Running");
        }

        public Task Consume(ConsumeContext<RegistrarContatoBO> context)
        {
            Console.WriteLine(context.Message);
            return Task.CompletedTask;
        }
    }
}
