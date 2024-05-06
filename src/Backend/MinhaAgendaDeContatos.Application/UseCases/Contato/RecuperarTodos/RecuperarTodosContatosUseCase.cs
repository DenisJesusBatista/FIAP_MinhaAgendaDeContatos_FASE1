using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
public class RecuperarTodosContatosUseCase : IRecuperarTodosContatosUseCase
{
    private readonly IContatoReadOnlyRepositorio _repositorioReadOnly;
    private readonly IMapper _mapper;

    public RecuperarTodosContatosUseCase(IContatoReadOnlyRepositorio repositorioReadOnly, IMapper mapper)
    {
        _mapper = mapper;
        _repositorioReadOnly = repositorioReadOnly;
    }

    public async Task<RespostaContatoJson> Executar()
    {
        var (contatos, regioes) = await _repositorioReadOnly.RecuperarTodosContatos();
                
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

        return new RespostaContatoJson { Contatos = contatosJson };
       
    }
}
