namespace MinhaAgendaDeContatos.Comunicacao.Requisicoes;
public class RequisicaoRegistrarContatoJson
{
   public int Id { get; set; } 
    public DateTime DataCriacao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; 
    public string Telefone { get; set; } = string.Empty;
    public string Prefixo { get; set; } = string.Empty;
}
