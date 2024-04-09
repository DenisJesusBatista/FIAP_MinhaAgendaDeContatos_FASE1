﻿using Microsoft.AspNetCore.Mvc;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;
using System.Reflection.Metadata.Ecma335;

namespace MinhaAgendaDeContatos.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ContatoController : ControllerBase
{ 

    [HttpPost]
    public async Task<IActionResult> RegistrarContato(
    [FromServices] IRegistrarContatoUseCase useCase,
    [FromBody] RequisicaoRegistrarContatoJson request)
    {
        // Aguardar a execução do método Executar
        await useCase.Executar(request);

        return Created(string.Empty, null);
    }


    [HttpGet]
    [Route("{prefixo}")]
    [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorPrefixo(
    [FromServices] IRecuperarPorPrefixoUseCase useCase,
    string prefixo)
    {
        var resposta = await useCase.Executar(prefixo);

        return Ok(resposta);
    }

    [HttpGet]    
    [ProducesResponseType(typeof(RespostaContatoRegistradoJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarTodosContatos(
        [FromServices] IRecuperarTodosContatosUseCase useCase)
    {
        var resposta = await useCase.Executar();

        return Ok(resposta);

    }

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