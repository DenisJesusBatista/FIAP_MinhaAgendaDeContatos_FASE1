namespace MinhaAgendaDeContatos.Comunicacao.Resposta;
public class RespostaContatoJson
{
    public List<ContatoJson> Contatos { get; set; }
}

public class ContatoJson
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Prefixo { get; set; } = string.Empty;
}
