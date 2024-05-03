using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MinhaAgendaDeContatos.Comunicacao.Resposta;
public class RespostaDDDRegiaoJson
{
    public List<DDDRegiaoJson> DDDRegiao { get; set; }
}

public class DDDRegiaoJson
{
    public int id { get; set; }
    public string prefixo { get; set; }
    public string regiao { get; set; }
    public string estado { get; set; }
}

