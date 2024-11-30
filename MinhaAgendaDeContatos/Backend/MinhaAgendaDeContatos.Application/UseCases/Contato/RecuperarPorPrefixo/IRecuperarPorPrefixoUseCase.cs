using MinhaAgendaDeContatos.Comunicacao.Resposta;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
public interface IRecuperarPorPrefixoUseCase
{
    Task<RespostaContatoJson> Executar(string prefixo);
}
