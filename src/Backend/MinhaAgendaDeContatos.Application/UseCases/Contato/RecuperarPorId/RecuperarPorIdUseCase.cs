using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using MinhaAgendaDeContatos.Exceptions;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
public class RecuperarPorIdUseCase : IRecuperarPorIdUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    private readonly IMapper _mapper;

    public RecuperarPorIdUseCase(IContatoReadOnlyRepositorio repositorioReadOnly, IMapper mapper)
    {
        _mapper = mapper;
        _repositorioReadOnly = repositorioReadOnly;

    }

    public async Task<RespostaContatoJson> Executar(int id)
    {
        //var contato = await _repositorioReadOnly.RecuperarPorId(id);
        var (contatos, regioes) = await _repositorioReadOnly.RecuperarPorId(id);

        //var resultado = contato.Select(c => _mapper.Map<ContatoJson>(c)).ToList();
        var contatosJson = contatos.Select(c => new ContatoJson
        {
            Id = (int)c.Id,
            DataCriacao = c.DataCriacao,
            Nome = c.Nome,
            Email = c.Email,
            Telefone = c.Telefone,
            Prefixo = c.Prefixo,
            // Mapeia a região correspondente ao prefixo do contato
            DDDRegiao = _mapper.Map<DDDRegiaoJson>(regioes.FirstOrDefault(r => r.prefixo == c.Prefixo))
        }).ToList();

        //Validar(contato);

        //return new RespostaContatoJson { Contatos = resultado };
        return new RespostaContatoJson { Contatos = contatosJson };
    }

    public static void Validar(IList<Domain.Entidades.Contato> contatos)
    {
        if (contatos == null || contatos.Count == 0)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.CONTATO_NAO_ENCONTRADO });
        }
    }

}
