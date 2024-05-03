using System.Linq;

namespace MinhaAgendaDeContatos.Comunicacao.Resposta;

public class RespostaContatoRegistradoJson
{
    /*Token que identifica o usuário, quando faz login da aplicação*/


    public class RespostaCombinadaJson
    {
        public List<DDDRegiaoJson> DDDRegiao { get; set; }
        public List<ContatoJson> Contatos { get; set; }
    }

    public class ContatoComDDDJson
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DDDRegiaoJson DDDRegiao { get; set; }
    }

    public string Token { get; set; } = string.Empty;

    public RespostaContatoRegistradoJson(string dddRegiao /* Outros tipos e campos necessários */)
    {
        //DDDRegiao = dddRegiao;
        // Inicialize outros campos, se necessário
    }


}
