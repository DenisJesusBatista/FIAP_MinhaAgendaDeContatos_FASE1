﻿using AutoMapper;
using FluentValidation.Results;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public class RegistrarContatoUseCase : IRegistrarContatoUseCase
{
    //Variavel readonly só pode ser atribuido valor nela, apenas no construtor da classe ( public RegistrarContatoUseCase(IContatoWriteOnlyRepositorio repositorio) )
    private readonly IContatoReadOnlyRepositorio _contatoReadOnlyRepositorio;
    private readonly IContatoWriteOnlyRepositorio _contatoWriteOnlyRepositorio;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    //Configurar a injeção de dependência atalho CTOR - Criar 
    //Construtor
    public RegistrarContatoUseCase(IContatoWriteOnlyRepositorio contatoWriteOnlyRepositorio, IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho,
       IContatoReadOnlyRepositorio contatoReadOnlyRepositorio
        )
    {
        _contatoWriteOnlyRepositorio = contatoWriteOnlyRepositorio;
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _contatoReadOnlyRepositorio = contatoReadOnlyRepositorio;
    }

    /// <summary>
    /// Consiste dados recebidos e persiste na base de dados 
    /// </summary>
    /// <param name="requisicao"></param>
    /// <returns>bool</returns>
    /// <exception cref="ErrosDeValidacaoException"></exception>
    public async Task<bool> Executar(RequisicaoRegistrarContatoJson requisicao)
    {
        bool result = true;
        var resultadoValidacao = await Validar(requisicao);
        result = resultadoValidacao.IsValid;

        //Resultado inválido retorna throw
        if (!result)
        {
            throw new ErrosDeValidacaoException(resultadoValidacao
                                                            .Errors
                                                            .Select(x => x.ErrorMessage)
                                                            .Distinct()
                                                            .ToList());

        }

        //Conversão requisicao para entidade AutoMap
        //-Pluggin: AutoMapper na Application
        //-Pluggin: AutoMapper.Extensions.Microsoft.DependencyInjection na API para configurar para funcionar como injecao de dependencia

        var entidade = _mapper.Map<Domain.Entidades.Contato>(requisicao);

        //Salvar no banco de dados

        await _contatoWriteOnlyRepositorio.Adicionar(entidade);
        await _unidadeDeTrabalho.Commit();

        return result;
    }

    /// <summary>
    /// Tem a responsabilidade de validar os dados 
    /// </summary>
    /// <param name="requisicao"></param>
    /// <returns>ValidationResult</returns>
    private async Task<ValidationResult> Validar(RequisicaoRegistrarContatoJson requisicao)
    {
        var validator = new RegistrarContatoValidator();
        ValidationResult? resultado = validator.Validate(requisicao);

        var existeContatoComEmail = await _contatoReadOnlyRepositorio.ExisteUsuarioComEmail(requisicao);

        if (existeContatoComEmail)
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_REGISTRADO));
        }

        return resultado;

        //if (!resultado.IsValid)
        //{
        //    var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
        //    throw new ErrosDeValidacaoException(mensagensDeErro);
        //}
    }
}
