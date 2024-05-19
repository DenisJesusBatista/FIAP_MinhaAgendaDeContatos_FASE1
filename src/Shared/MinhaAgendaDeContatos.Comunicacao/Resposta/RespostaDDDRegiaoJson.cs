using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Comunicacao.Resposta;
[ExcludeFromCodeCoverage]
public class RespostaDDDRegiaoJson
{
    public List<DDDRegiaoJson> DDDRegiao { get; set; }
}

public class DDDRegiaoJson
{
    public long Id { get; set; }
    public string Prefixo { get; set; }
    public string Regiao { get; set; }
    public string Estado { get; set; }
}

