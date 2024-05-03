namespace MinhaAgendaDeContatos.Domain.Entidades;

public class Contato : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;    
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;   
    public string Prefixo { get; set; } = string.Empty;

    //public List<DDDRegiao> DDDRegiao { get; set; }
}
