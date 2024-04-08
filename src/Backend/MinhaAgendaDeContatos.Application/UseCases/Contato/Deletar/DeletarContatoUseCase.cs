using AutoMapper;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
public class DeletarContatoUseCase : IDeletarContatoUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    private readonly IContatoWriteOnlyRepositorio _repositorioWriteOnly; 
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public DeletarContatoUseCase(
        IContatoReadOnlyRepositorio repositorioReadOnly, 
        IContatoWriteOnlyRepositorio repositorioWriteOnly,          
        IUnidadeDeTrabalho unidadeDeTrabalho)
    {      
        _repositorioReadOnly = repositorioReadOnly;
        _repositorioWriteOnly = repositorioWriteOnly;
        _unidadeDeTrabalho = unidadeDeTrabalho; 
    }

    public async Task Executar(string email)
    {
        var contato = await _repositorioReadOnly.RecuperarPorEmail(email);

        Validar(contato);

        await _repositorioWriteOnly.Deletar(email);

        await _unidadeDeTrabalho.Commit();

    }

    public static void Validar(Domain.Entidades.Contato contato)
    {
        if (contato == null)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.CONTATO_EMAIL_NAO_ENCONTRADO });
        }
    }
}
