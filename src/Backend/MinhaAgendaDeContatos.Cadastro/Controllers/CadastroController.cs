using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;

namespace MinhaAgendaDeContatos.Cadastro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CadastroController : ControllerBase
    {
        private readonly IRabbitMqProducer _rabbitMqProducer;

        public CadastroController(IRabbitMqProducer rabbitMqProducer)
        {
            _rabbitMqProducer = rabbitMqProducer;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarContato([FromBody] RequisicaoRegistrarContatoJson request)
        {
            try
            {
                // Crie um objeto com a ação e os dados
                var message = new { Acao = "Registro", Dados = request };

                // Defina o nome da fila onde a mensagem será publicada
                var queueName = "registrarContato"; // Substitua pelo nome da fila desejada

                // Envie o objeto para o RabbitMqProducer
                _rabbitMqProducer.PublishMessage(queueName, message); // Passa o nome da fila e a mensagem

                return Ok(ResponseMessages.ContatoCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}