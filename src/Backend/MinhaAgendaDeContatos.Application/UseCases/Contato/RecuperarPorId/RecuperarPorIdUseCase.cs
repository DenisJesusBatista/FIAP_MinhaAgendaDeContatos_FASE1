using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
public class RecuperarPorIdUseCase : IRecuperarPorIdUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    public RecuperarPorIdUseCase(IContatoReadOnlyRepositorio repositorioReadOnly, IMapper mapper)
    {
        _repositorioReadOnly = repositorioReadOnly;
    }

    public async Task<RespostaContatoJson> Executar(int id)
    {
        var contatos = await _repositorioReadOnly.RecuperarPorId(id);

        return RespostaContatoJson.FromEntity(contatos);
    }
}
