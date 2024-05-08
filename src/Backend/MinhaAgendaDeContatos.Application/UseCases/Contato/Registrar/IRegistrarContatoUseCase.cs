using MinhaAgendaDeContatos.Comunicacao.Requisicoes;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public interface IRegistrarContatoUseCase
{
    Task Executar(RequisicaoRegistrarContatoJson requisicao);
}
