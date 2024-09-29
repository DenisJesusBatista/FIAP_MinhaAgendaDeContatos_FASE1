namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
public interface IDeletarContatoUseCase
{
    Task<bool> Executar(string email);
}
