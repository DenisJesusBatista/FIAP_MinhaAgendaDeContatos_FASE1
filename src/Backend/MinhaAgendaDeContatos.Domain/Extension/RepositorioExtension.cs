using Microsoft.Extensions.Configuration;

namespace MinhaAgendaDeContatos.Domain.Extension;
public static class RepositorioExtension
{
    public static string GetNomeDataBase(this IConfiguration configuration)
    {
        // Obtenha o nome do banco de dados a partir da configuração ou variável de ambiente
        var nomeDatabase = configuration["DB_NAME"] ?? configuration.GetConnectionString("NomeDatabase");

        return nomeDatabase;
    }

    public static string GetConexao(this IConfiguration configuration)
    {
        // Obtenha a string de conexão completa a partir da configuração ou variáveis de ambiente
        var userId = configuration["DB_USER"] ?? "postgres";
        var password = configuration["DB_PASSWORD"] ?? "postgres";
        var host = configuration["DB_HOST"] ?? "localhost";
        var port = configuration["DB_PORT"] ?? "5432";
        var database = GetNomeDataBase(configuration);

        return $"User ID={userId};Password={password};Host={host};Port={port};Database={database};";
    }

    public static string GetConexaoCompleta(this IConfiguration configuration)
    {
        var conexao = GetConexao(configuration);

        return conexao;
    }
}
