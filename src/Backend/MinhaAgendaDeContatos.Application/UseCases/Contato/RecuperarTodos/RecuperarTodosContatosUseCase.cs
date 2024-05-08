using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
public class RecuperarTodosContatosUseCase : IRecuperarTodosContatosUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    public RecuperarTodosContatosUseCase(IContatoReadOnlyRepositorio repositorioReadOnly, IMapper mapper)
    {
        _repositorioReadOnly = repositorioReadOnly;
    }

    public async Task<RespostaContatoJson> Executar()
    {
        var contatos = await _repositorioReadOnly.RecuperarTodosContatos();

        return RespostaContatoJson.FromEntity(contatos);
    }
}
