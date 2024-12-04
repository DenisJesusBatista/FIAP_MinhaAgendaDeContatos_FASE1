namespace MinhaAgendaDeContatos.Comunicacao.Requisicoes;
public class RequisicaoAlterarContatoJson
{
    public string EmailAtual { get; set; } = string.Empty;
    public string EmailNovo { get; set; } = string.Empty;
    public string ContatoAtual { get; set; } = string.Empty;
    public string ContatoNovo { get; set; } = string.Empty;
}
