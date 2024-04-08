using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Domain.Entidades;

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
        var contato = await _repositorioReadOnly.RecuperarTodosContatos();

        var contatosJson = contato.Select(c => _mapper.Map<ContatoJson>(c)).ToList();

        return new RespostaContatoJson { Contatos = contatosJson };
       
    }
}
