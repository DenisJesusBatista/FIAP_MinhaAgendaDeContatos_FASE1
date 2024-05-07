using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Application.UseCases.DDDRegiao.RecuperarPorPrefixo;
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
    [FromBody] RequisicaoRegistrarContatoJson request,
    IRecuperarDDDRegiaoPorPrefixoUseCase useCaseDDDRegiao)
    {
        // Chamar o caso de uso para registrar o contato
        var entidade = await useCase.Executar(request);

        var dddRegioes = await useCaseDDDRegiao.Executar(request.Prefixo);

        // Criar um objeto ContatoJson com as informações do contato e do DDD correspondente
        var contatoComDDD = new ContatoJson
        {   
            DataCriacao = entidade.DataCriacao,
            Nome = entidade.Nome,
            Email = entidade.Email,
            Telefone = entidade.Telefone,
            Prefixo = entidade.Prefixo,
            DDDRegiao = dddRegioes.DDDRegiao.FirstOrDefault()
        };
  

        return Ok(contatoComDDD);
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
    [Route("id/{id}")]
    [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorId(
    [FromServices] IRecuperarPorIdUseCase useCase,
    IRecuperarDDDRegiaoPorPrefixoUseCase useCaseDDDRegiao,
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
        [FromBody] string email)
    {
        await useCase.Executar(email);

        return NoContent();

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
