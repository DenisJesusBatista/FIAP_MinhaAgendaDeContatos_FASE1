using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;



using static MinhaAgendaDeContatos.Comunicacao.Resposta.RespostaContatoRegistradoJson;

namespace MinhaAgendaDeContatos.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ContatoController : ControllerBase
{
    /// <summary>
    /// Registrar contato no banco de dados
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RegistrarContato(
    [FromServices] IRegistrarContatoUseCase useCase,
    [FromBody] RequisicaoRegistrarContatoJson request)
    {
        await useCase.Executar(request);

        return Ok(ResponseMessages.ContatoCriado);
    }

    [HttpGet]
    [Route("prefixo/{prefixo}")]
    [ProducesResponseType(typeof(RespostaCombinadaJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorPrefixo(
    [FromServices] IRecuperarPorPrefixoUseCase useCase,
    string prefixo)
    {
        var respostaContato = await useCase.Executar(prefixo);

        return Ok(respostaContato);
    }

    /// <summary>
    /// Retorna os contatos de acordo com o id informado.
    /// </summary>
    /// <param name = "useCase" ></ param >
    /// < param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorId(
    [FromServices] IRecuperarPorIdUseCase useCase,
    int id)
    {
        var resposta = await useCase.Executar(id);

        return Ok(resposta);
    }

    /// <summary>
    /// Retornar todos os contatos.
    /// </summary>
    /// <param name="useCase"></param>
    /// <returns></returns>

    [HttpGet]
    [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarTodosContatos(
    [FromServices] IRecuperarTodosContatosUseCase useCase)
    {
        var resposta = await useCase.Executar();

        return Ok(resposta);
    }

    /// <summary>
    /// Deletar contato do banco de dados através do email informado.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{email:}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deletar(
        [FromServices] IDeletarContatoUseCase useCase,
        string email)
    {
        await useCase.Executar(email);

        return Ok(ResponseMessages.ContatoApagado);
    }

    /// <summary>
    /// Atualizar o telefone ou email do contato.
    /// </summary>
    /// <param name="useCase"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateContato(
       [FromServices] IUpdateContatoUseCase useCase,
       [FromBody] RequisicaoAlterarContatoJson request)
    {
        await useCase.Executar(request);

        return NoContent();
    }



}
