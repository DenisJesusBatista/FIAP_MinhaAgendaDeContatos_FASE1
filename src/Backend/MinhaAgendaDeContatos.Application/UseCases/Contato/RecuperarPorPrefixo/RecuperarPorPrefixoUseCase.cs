using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;
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
        // Recupera contatos e regiões por prefixo
        var (contatos, regioes) = await _repositorioReadOnly.RecuperarPorPrefixo(prefixo);      

    
        // Mapeia os contatos para ContatoJson
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

        //// Valida os contatos e regiões, se necessário
        //Validar(contatosJson);      

        return new RespostaContatoJson { Contatos = contatosJson};
    }




    public static void Validar(IList<Domain.Entidades.Contato> contatos)
    {
        if (contatos == null || contatos.Count == 0)
        {
            throw new ErrosDeValidacaoException(new List<string> { ResourceMensagensDeErro.CONTATO_PREFIXO_NAO_ENCONTRADO });
        }
    }
}
