using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Domain.Entidades;
[ExcludeFromCodeCoverage]
public class EntidadeBase
{
    public long Id { get; set; }
    public DateTime DataCriacao { get; set; }
}
