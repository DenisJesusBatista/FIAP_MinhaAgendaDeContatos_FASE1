using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
public class UpdateContatoUseCase: IUpdateContatoUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    private readonly IContatoWriteOnlyRepositorio _repositorioWriteOnly;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public UpdateContatoUseCase(
       IContatoReadOnlyRepositorio repositorioReadOnly,
       IContatoWriteOnlyRepositorio repositorioWriteOnly,
       IUnidadeDeTrabalho unidadeDeTrabalho)
    
    {
        _repositorioReadOnly = repositorioReadOnly;
        _repositorioWriteOnly = repositorioWriteOnly;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }  

    public async Task Executar(RequisicaoAlterarContatoJson requisicao)
    {

        var contato = await _repositorioReadOnly.RecuperarPorEmail(requisicao.EmailAtual);

        Validar(contato);

        contato.Email = (string.IsNullOrEmpty(requisicao.EmailNovo) || requisicao.EmailNovo == "string") ? contato.Email : requisicao.EmailNovo;
        contato.Telefone = (string.IsNullOrEmpty(requisicao.ContatoNovo) || requisicao.ContatoNovo == "string") ? contato.Telefone : requisicao.ContatoNovo;

        contato.DataCriacao = contato.DataCriacao.ToUniversalTime();

        await _repositorioWriteOnly.Update(contato);

        await _unidadeDeTrabalho.Commit();
    }

    public static void Validar(Domain.Entidades.Contato contato)
    {
        if (contato == null)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.CONTATO_NAO_ENCONTRADO });
        }       
    }
}
