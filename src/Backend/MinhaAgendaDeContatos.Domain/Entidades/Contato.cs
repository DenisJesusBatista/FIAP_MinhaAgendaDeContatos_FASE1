using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Domain.Entidades;

[ExcludeFromCodeCoverage]
public class Contato : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Prefixo { get; set; } = string.Empty;
    public DDDRegiao Regiao { get; set; }
}
