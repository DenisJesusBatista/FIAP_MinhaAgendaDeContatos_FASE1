using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
public class RecuperarPorPrefixoUseCase : IRecuperarPorPrefixoUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    public RecuperarPorPrefixoUseCase(IContatoReadOnlyRepositorio repositorioReadOnly, IMapper mapper)
    {
        _repositorioReadOnly = repositorioReadOnly;
    }

    public async Task<RespostaContatoJson> Executar(string prefixo)
    {
        var contatos = await _repositorioReadOnly.RecuperarPorPrefixo(prefixo);

        return RespostaContatoJson.FromEntity(contatos);
    }
}
