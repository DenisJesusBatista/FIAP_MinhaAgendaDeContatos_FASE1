using Dapper;
using Npgsql;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations;
public static class Database
{   
    public static void CriarDatabase(string conexaoComBancoDeDados, string nomeDatabase)
    {
        using var minhaConexao = new NpgsqlConnection(conexaoComBancoDeDados);

        var parametros = new DynamicParameters();
        parametros.Add("nome", nomeDatabase);

        // Consulta para verificar se o banco de dados já existe
        string consulta = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.SCHEMATA WHERE CATALOG_NAME = @nome";

        // ExecuteScalar retorna o número de registros retornados pela consulta
        int quantidadeDeRegistros = minhaConexao.ExecuteScalar<int>(consulta, parametros);

        // Se não houver registros, o banco de dados não existe
        if (quantidadeDeRegistros == 0)
        {
            // Criação do banco de dados
            string comandoCriarDatabase = $"CREATE DATABASE {nomeDatabase}";
            minhaConexao.Execute(comandoCriarDatabase);
        }
    }

    public static async Task<bool> VerificarExistenciaDatabaseAsync(string conexaoComBancoDeDados, string nomeDatabase)
    {
        using var minhaConexao = new NpgsqlConnection(conexaoComBancoDeDados);

        var parametros = new DynamicParameters();
        parametros.Add("nome", nomeDatabase);

        // Aguarda 10 segundos antes de continuar
        //await Task.Delay(10000);

        // Consulta para verificar se o banco de dados já existe
        string consulta = "SELECT COUNT(*) FROM pg_database WHERE datname = @nome";

        // ExecuteScalarAsync retorna o número de registros retornados pela consulta
        int quantidadeDeRegistros = await minhaConexao.ExecuteScalarAsync<int>(consulta, parametros);

        // Se a quantidade de registros for maior que 0, o banco de dados existe
        return quantidadeDeRegistros > 0;
    }




}
