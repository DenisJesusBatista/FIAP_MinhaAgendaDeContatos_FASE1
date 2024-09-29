using MinhaAgendaDeContatos.Comunicacao.Requisicoes;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public interface IRegistrarContatoUseCase
{
    Task<bool> Executar(RequisicaoRegistrarContatoJson requisicao);
}
