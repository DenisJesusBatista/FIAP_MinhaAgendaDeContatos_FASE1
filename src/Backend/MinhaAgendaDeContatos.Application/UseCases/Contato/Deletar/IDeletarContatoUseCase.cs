namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
public interface IDeletarContatoUseCase
{
    Task Executar(string email);
}
