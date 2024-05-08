namespace MinhaAgendaDeContatos.Comunicacao.Resposta;
public class RespostaErroJson
{
    public List<string> Mensagens { get; set; }

    public RespostaErroJson(string mensagem)
    {
        Mensagens = new List<string>
        {
            mensagem
        };
    }

    public RespostaErroJson(List<string> mensagem)
    {
        Mensagens = mensagem;

    }

}

