using System.ComponentModel.DataAnnotations;

namespace MinhaAgendaDeContatos.Domain.Entidades;
public class DDDRegiao : EntidadeBase
{
    [Key]
    public string Prefixo { get; set; } = string.Empty;
    public string Regiao { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public ICollection<Contato> Contatos { get; set; }
}
