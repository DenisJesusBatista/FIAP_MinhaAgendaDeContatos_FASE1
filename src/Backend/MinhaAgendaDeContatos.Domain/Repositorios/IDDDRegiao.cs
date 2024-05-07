namespace MinhaAgendaDeContatos.Domain.Repositorios;
public interface IDDDRegiao
{
    Task<IList<Entidades.DDDRegiao>> RecuperarPorPrefixo(string prefixo);
}
