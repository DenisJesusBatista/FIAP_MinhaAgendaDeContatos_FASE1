using MinhaAgendaDeContatos.Domain.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Comunicacao.Resposta;
[ExcludeFromCodeCoverage]
public class RespostaContatoJson
{
    public IEnumerable<ContatoJson> Contatos { get; set; }

    public static RespostaContatoJson FromEntity(IEnumerable<Contato> contatos)
    {
        return new RespostaContatoJson() { Contatos = contatos.Select(x => ContatoJson.FromEntity(x)) };
    }
}

[ExcludeFromCodeCoverage]
public class ContatoJson
{
    public long Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Prefixo { get; set; }
    public DDDRegiaoJson DDDRegiao { get; set; }

    public static ContatoJson FromEntity(Contato contato)
    {
        return new ContatoJson()
        {
            Id = contato.Id,
            DataCriacao = contato.DataCriacao,
            Nome = contato.Nome,
            Email = contato.Email,
            Telefone = contato.Telefone,
            DDDRegiao = new DDDRegiaoJson()
            {
                Id = contato.Regiao.Id,
                Prefixo = contato.Regiao.Prefixo,
                Regiao = contato.Regiao.Regiao,
                Estado = contato.Regiao.Estado,
            }
        };
    }
}

