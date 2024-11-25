using MinhaAgendaDeContatos.Domain.Entidades;

namespace MinhaAgendaDeContatos.Comunicacao.Resposta
{
    // Classe principal para a resposta
    public class RespostaContatoRegistradoJson
    {
        public string Token { get; set; } = string.Empty;

        // Tornando as propriedades anuláveis ou inicializando com listas vazias
        public List<DDDRegiaoJson> DDDRegiao { get; set; } = new List<DDDRegiaoJson>();
        public List<ContatoJson> Contatos { get; set; } = new List<ContatoJson>();

        // Construtor para inicializar as propriedades
        public RespostaContatoRegistradoJson(List<DDDRegiaoJson> dddRegiao, List<ContatoJson> contatos)
        {
            DDDRegiao = dddRegiao ?? new List<DDDRegiaoJson>(); // Garantindo que a lista nunca seja nula
            Contatos = contatos ?? new List<ContatoJson>();    // Garantindo que a lista nunca seja nula
        }
    }

    // Classe RespostaCombinadaJson movida para fora, caso queira usá-la de forma independente
    public class RespostaCombinadaJson
    {
        public List<DDDRegiaoJson> DDDRegiao { get; set; } = new List<DDDRegiaoJson>(); // Inicializa com lista vazia
        public List<ContatoJson> Contatos { get; set; } = new List<ContatoJson>(); // Inicializa com lista vazia

        // Construtor
        public RespostaCombinadaJson(List<DDDRegiaoJson> dddRegiao, List<ContatoJson> contatos)
        {
            DDDRegiao = dddRegiao ?? new List<DDDRegiaoJson>();
            Contatos = contatos ?? new List<ContatoJson>();
        }
    }
}
