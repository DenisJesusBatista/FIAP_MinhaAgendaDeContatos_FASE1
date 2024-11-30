using Microsoft.Extensions.Configuration;

namespace MinhaAgendaDeContatos.Domain.Extension;
public static class RepositorioExtension
{
    public static string GetNomeDataBase(this IConfiguration configurationManager)
    {
        var nomeDatabase = configurationManager.GetConnectionString("NomeDatabase");

        return nomeDatabase;

    }

    public static string GetConexao(this IConfiguration configurationManager)
    {
        var conexao = configurationManager.GetConnectionString("Conexao");

        return conexao;

    }

    public static string GetConexaoCompleta(this IConfiguration configurationManager)
    {
        var nomeDatabase = configurationManager.GetNomeDataBase();
        var conexao = configurationManager.GetConexao();

        return $"{conexao}Database={nomeDatabase}";
    }
}
