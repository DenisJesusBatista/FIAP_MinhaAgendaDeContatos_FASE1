using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;

namespace MinhaAgendaDeContatos.Orchestrator.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRabbitMqProducer _rabbitMqProducer;
        private readonly IRabbitMqConsumer _rabbitMqConsumer;

        public HomeController(ILogger<HomeController> logger,
                                         IRabbitMqConsumer rabbitMqConsumer,
                                         IRabbitMqProducer rabbitMqProducer)
        {
            _logger = logger;
            _rabbitMqConsumer = rabbitMqConsumer;
        }

        [HttpGet(Name = "Start")]
        public IActionResult Get()
        {
           // _rabbitMqConsumer.ConsumeMessage();

            return Ok();
        }
    }
}
