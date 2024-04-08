using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
public class RecuperarPorPrefixoUseCase: IRecuperarPorPrefixoUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    private readonly IMapper _mapper;

    public RecuperarPorPrefixoUseCase(IContatoReadOnlyRepositorio repositorioReadOnly, IMapper mapper)
    {
        _mapper = mapper;
        _repositorioReadOnly = repositorioReadOnly;        
    }

    public async Task<RespostaContatoJson> Executar(string prefixo)
    {
        var contato = await _repositorioReadOnly.RecuperarPorPrefixo(prefixo);

        var resultado = contato.Select(c => _mapper.Map<ContatoJson>(c)).ToList();

        Validar(contato);     

        return new RespostaContatoJson { Contatos = resultado };
    }

    public static void Validar(IList<Domain.Entidades.Contato> contatos)
    {
        if (contatos == null || contatos.Count == 0)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.CONTATO_PREFIXO_NAO_ENCONTRADO });
        }
    }
}
