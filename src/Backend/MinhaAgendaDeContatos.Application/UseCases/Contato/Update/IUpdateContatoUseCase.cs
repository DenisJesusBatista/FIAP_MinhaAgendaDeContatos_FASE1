using MinhaAgendaDeContatos.Comunicacao.Requisicoes;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
public interface IUpdateContatoUseCase
{
    Task<bool> Executar(RequisicaoAlterarContatoJson requisicao);
}
