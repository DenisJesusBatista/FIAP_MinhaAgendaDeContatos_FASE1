namespace MinhaAgendaDeContatos.Domain.Entidades;
public class EntidadeBase
{
    public long Id { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
