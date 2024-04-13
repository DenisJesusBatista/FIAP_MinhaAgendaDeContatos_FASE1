using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MinhaAgendaDeContatos.Comunicacao.Requisicoes;

/// <summary>
/// classe proxy para fazer o De/Para com a entidade
/// </summary>

[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Skip)]
public class RequisicaoRegistrarContatoJson()
{
    //public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public string Nome { get; set; } = string.Empty;

    private string _email = string.Empty;
    public string Email
    {
        get
        {
            return _email;
        }
        set
        {
            _email = value.ToLower().Trim();
        }
    }
    [JsonIgnore]
    public string Telefone
    {
        get
        {
            return TelefoneProxy.ToString();
        }
    }

    public ulong TelefoneProxy { get; set; } = 0;   
    [JsonIgnore]
    public string Prefixo
    {
        get
        {
            return PrefixoProxy.ToString();
        }
    }

    public ulong PrefixoProxy { get; set; } = 0;
}
