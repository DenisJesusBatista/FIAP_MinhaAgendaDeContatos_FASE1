using MinhaAgendaDeContatos.Comunicacao.Resposta;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
public interface IRecuperarPorIdUseCase
{
    Task<RespostaContatoJson> Executar(int id);
}
