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
using MinhaAgendaDeContatos.Domain.Entidades;
using System.Linq;
using System;



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
    IRecuperarDDDRegiaoPorPrefixoUseCase useCaseDDDRegiao,
    string prefixo)
    {
        var respostaContato = await useCase.Executar(prefixo);
        var dddRegioes = await useCaseDDDRegiao.Executar(prefixo);

        // Mapeando os contatos para adicionar o DDD correspondente
        var contatosComDDD = respostaContato.Contatos.Select(contato =>
        {
            // Encontrando o DDD correspondente ao prefixo do contato
            var dddCorrespondente = dddRegioes.DDDRegiao.FirstOrDefault(ddd => ddd.prefixo == contato.Prefixo);

            return new ContatoJson // Alterado para ContatoJson
            {
                Id = contato.Id,
                DataCriacao = contato.DataCriacao,
                Nome = contato.Nome,
                Email = contato.Email,
                Telefone = contato.Telefone,
                Prefixo = contato.Prefixo,
                DDDRegiao = dddCorrespondente
            };
        }).ToList();        

        return Ok(contatosComDDD);
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

        // Obter o prefixo do contato
        var prefixo = resposta.Contatos.FirstOrDefault()?.Prefixo;


        var dddRegioes = await useCaseDDDRegiao.Executar(prefixo);

        // Mapeando os contatos para adicionar o DDD correspondente
        var contatosComDDD = resposta.Contatos.Select(contato =>
        {
            // Encontrando o DDD correspondente ao prefixo do contato
            var dddCorrespondente = dddRegioes.DDDRegiao.FirstOrDefault(ddd => ddd.prefixo == contato.Prefixo);


            return new ContatoJson // Alterado para ContatoJson
            {
                Id = contato.Id,
                DataCriacao = contato.DataCriacao,
                Nome = contato.Nome,
                Email = contato.Email,
                Telefone = contato.Telefone,
                Prefixo = contato.Prefixo,
                DDDRegiao = dddCorrespondente
            };
        }).ToList();

        return Ok(contatosComDDD);


    }

    /// <summary>
    /// Retornar todos os contatos.
    /// </summary>
    /// <param name="useCase"></param>
    /// <returns></returns>

    [HttpGet]
    [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarTodosContatos(
    [FromServices] IRecuperarTodosContatosUseCase useCase,
    IRecuperarDDDRegiaoPorPrefixoUseCase useCaseDDDRegiao)
    {
        var resposta = await useCase.Executar();
        var prefixos = resposta.Contatos.Select(x => x.Prefixo).Distinct().ToList();

        // Lista para armazenar todos os contatos com DDD correspondente não nulo
        var contatosComDDD = new List<ContatoJson>();

        // Iterar sobre os prefixos
        foreach (var prefixo in prefixos)
        {
            // Executar o caso de uso para obter as informações do DDD correspondente ao prefixo atual
            var dddRegioes = await useCaseDDDRegiao.Executar(prefixo);

            // Iterar sobre os contatos e encontrar o DDD correspondente ao prefixo atual
            foreach (var contato in resposta.Contatos)
            {
                // Encontrando o DDD correspondente ao prefixo do contato
                var dddCorrespondente = dddRegioes.DDDRegiao.FirstOrDefault(ddd => ddd.prefixo == contato.Prefixo);

                // Verificar se o DDD correspondente não é nulo
                if (dddCorrespondente != null)
                {
                    // Criando um objeto ContatoJson com as informações do contato e do DDD correspondente
                    var contatoComDDD = new ContatoJson
                    {
                        Id = contato.Id,
                        DataCriacao = contato.DataCriacao,
                        Nome = contato.Nome,
                        Email = contato.Email,
                        Telefone = contato.Telefone,
                        Prefixo = contato.Prefixo,
                        DDDRegiao = dddCorrespondente
                    };

                    // Adicionando o contato com DDD correspondente à lista de contatosComDDD
                    contatosComDDD.Add(contatoComDDD);
                }
            }
        }

        return Ok(contatosComDDD);
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
