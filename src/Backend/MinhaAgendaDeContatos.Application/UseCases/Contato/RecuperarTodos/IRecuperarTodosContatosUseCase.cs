using MinhaAgendaDeContatos.Comunicacao.Resposta;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
public interface IRecuperarTodosContatosUseCase
{
    Task<RespostaContatoJson> Executar();
}
