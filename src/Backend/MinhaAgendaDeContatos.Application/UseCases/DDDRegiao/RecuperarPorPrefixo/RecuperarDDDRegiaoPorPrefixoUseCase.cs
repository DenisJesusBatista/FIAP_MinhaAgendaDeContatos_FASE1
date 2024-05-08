using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Application.UseCases.DDDRegiao.RecuperarPorPrefixo;
public class RecuperarDDDRegiaoPorPrefixoUseCase : IRecuperarDDDRegiaoPorPrefixoUseCase
{
    private readonly IDDDRegiao _repositorio;
    private readonly IMapper _mapper;

    public RecuperarDDDRegiaoPorPrefixoUseCase(IDDDRegiao repositorio, IMapper mapper)
    {
        _mapper = mapper;
        _repositorio = repositorio;
    }


    public async Task<RespostaDDDRegiaoJson> Executar(string prefixo)
    {
        var dddRegiao = await _repositorio.RecuperarPorPrefixo(prefixo);

        var resultado = dddRegiao.Select(c => _mapper.Map<DDDRegiaoJson>(c)).ToList();

        return new RespostaDDDRegiaoJson { DDDRegiao = resultado };
    }
}
