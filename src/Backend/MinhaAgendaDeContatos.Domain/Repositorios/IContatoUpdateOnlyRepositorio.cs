using MinhaAgendaDeContatos.Domain.Entidades;

namespace MinhaAgendaDeContatos.Domain.Repositorios;
public interface IContatoUpdateOnlyRepositorio
{    
    Task<Entidades.Contato> RecuperarPorEmail(string email);

    void Update(Entidades.Contato contato);
}
