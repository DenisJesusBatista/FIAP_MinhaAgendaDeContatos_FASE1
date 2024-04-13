using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public interface IRegistrarContatoUseCase
{    
    Task<bool> Executar(RequisicaoRegistrarContatoJson requisicao);
}
