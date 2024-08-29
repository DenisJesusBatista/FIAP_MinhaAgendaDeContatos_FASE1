using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;

namespace MinhaAgendaDeContatos.Alteracao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlteracaoController : ControllerBase
    {
        private readonly IRabbitMqProducer _rabbitMqProducer;

        public AlteracaoController(IRabbitMqProducer rabbitMqProducer)
        {
            _rabbitMqProducer = rabbitMqProducer;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContato([FromBody] RequisicaoAlterarContatoJson request)
        {
            try
            {
                // Crie um objeto com a ação e os dados
                var message = new { Acao = "Alteracao", Dados = request };

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

