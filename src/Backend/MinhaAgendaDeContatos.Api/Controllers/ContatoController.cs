using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using System;
using System.Threading.Tasks;
using static MinhaAgendaDeContatos.Comunicacao.Resposta.RespostaContatoRegistradoJson;

namespace MinhaAgendaDeContatos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IRabbitMqProducer _rabbitMqProducer;
        private readonly ILogger<ContatoController> _logger;

        // Construtor para injeção de dependências
        public ContatoController(IRabbitMqProducer rabbitMqProducer, ILogger<ContatoController> logger)
        {
            _rabbitMqProducer = rabbitMqProducer;
            _logger = logger;
        }

        /// <summary>
        /// Registrar contato no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarContato([FromBody] RequisicaoRegistrarContatoJson request)
        {
            try
            {
                var message = new { Acao = "Registro", Dados = request };
                var queueName = "registrarContato";

                await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                return Ok(ResponseMessages.ContatoCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar contato");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Recuperar contatos por prefixo
        /// </summary>
        /// <param name="prefixo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("prefixo/{prefixo}")]
        [ProducesResponseType(typeof(RespostaCombinadaJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> RecuperarPorPrefixo(string prefixo)
        {
            try
            {
                var message = new { Acao = "RecuperarPorPrefixo", Dados = prefixo };
                var queueName = "recuperarPorPrefixo";

                await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                return Ok(new { Status = "Mensagem enviada para recuperação por prefixo" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar contatos por prefixo");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Recuperar contato por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> RecuperarPorId(int id)
        {
            try
            {
                var message = new { Acao = "RecuperarPorId", Dados = id };
                var queueName = "recuperarPorId";

                await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                return Ok(new { Status = "Mensagem enviada para recuperação por ID" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar contato por ID");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Recuperar todos os contatos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> RecuperarTodosContatos()
        {
            try
            {
                var message = new { Acao = "RecuperarTodosContatos", Dados = (object)null };
                var queueName = "recuperarTodosContatos";

                await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                return Ok(new { Status = "Mensagem enviada para recuperação de todos os contatos" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar todos os contatos");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletar contato por email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{email}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Deletar(string email)
        {
            try
            {
                var message = new { Acao = "Delecao", Dados = email };
                var queueName = "DelecaoContato";

                await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar contato");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Atualizar contato
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateContato([FromBody] RequisicaoAlterarContatoJson request)
        {
            try
            {
                var message = new { Acao = "Alteracao", Dados = request };
                var queueName = "AlteracaoContato";

                await _rabbitMqProducer.PublishMessageAsync(queueName, message);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar contato");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
