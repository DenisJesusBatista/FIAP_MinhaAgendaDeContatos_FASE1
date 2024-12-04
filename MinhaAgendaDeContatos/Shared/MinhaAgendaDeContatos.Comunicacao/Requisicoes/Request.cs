using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Comunicacao.Requisicoes
{
    // Classe principal que contém o JSON
    public class RootObject
    {
        public string Id { get; set; } // Utiliza Guid para o formato UUID
        public Payload Payload { get; set; } // Propriedade do tipo Payload
    }

    // Classe Payload que está dentro do JSON
    public class Payload
    {
        public string Acao { get; set; } // Propriedade "Acao" do tipo string
        public RequisicaoRegistrarContatoJson Dados { get; set; } // Propriedade do tipo Dados
    }
}
