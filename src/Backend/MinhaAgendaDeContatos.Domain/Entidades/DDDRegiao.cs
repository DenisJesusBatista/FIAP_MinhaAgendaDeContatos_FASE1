using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Domain.Entidades;
public class DDDRegiao: EntidadeBase
{   
    public string prefixo { get; set; } = string.Empty; 
    public string regiao { get; set; } = string.Empty;
    public string estado { get; set; } = string.Empty;
}
