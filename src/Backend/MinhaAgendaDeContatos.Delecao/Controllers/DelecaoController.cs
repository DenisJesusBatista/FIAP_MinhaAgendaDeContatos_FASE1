using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;

namespace MinhaAgendaDeContatos.Delecao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DelecaoController : ControllerBase
    {
        private readonly IRabbitMqProducer _rabbitMqProducer;

        public DelecaoController(IRabbitMqProducer rabbitMqProducer)
        {
            _rabbitMqProducer = rabbitMqProducer;
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Deletar(string email)
        {
            try
            {
                var message = new { Acao = "Delecao", Dados = email };

                // Defina o nome da fila onde a mensagem será publicada
                var queueName = "AlteracaoContato"; // Substitua pelo nome da fila desejada

                // Envie o objeto para o RabbitMqProducer
                _rabbitMqProducer.PublishMessage(queueName, message); // Passa o nome da fila e a mensagem
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
