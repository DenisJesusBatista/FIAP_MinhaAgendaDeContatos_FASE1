using Dapper;
using Npgsql;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations;
public static class Database
{
    //public static void CriarDatabase(string conexaoComBancoDeDados, string nomeDatabase)
    //{
    //    using var minhaConexao = new NpgsqlConnection(conexaoComBancoDeDados);

    //    var parametros = new DynamicParameters();
    //    parametros.Add("nome", nomeDatabase);

    //    /*Verificar se existe dataBase*/

    //    var registros = minhaConexao.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE CATALOG_NAME = @nome", parametros);

    //    if (!registros.Any())
    //    {
    //        /*string interpolation Exemplo: ($"CREATE DATABASE {nomeDatabase}")*/
    //        minhaConexao.Execute($"CREATE DATABASE {nomeDatabase}");
    //    }

    //}

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

    public static bool VerificarExistenciaDatabase(string conexaoComBancoDeDados, string nomeDatabase)
    {
        using var minhaConexao = new NpgsqlConnection(conexaoComBancoDeDados);

        var parametros = new DynamicParameters();
        parametros.Add("nome", nomeDatabase);

        // Consulta para verificar se o banco de dados já existe
        string consulta = "SELECT COUNT(*) FROM pg_database WHERE datname = @nome";

        // ExecuteScalar retorna o número de registros retornados pela consulta
        int quantidadeDeRegistros = minhaConexao.ExecuteScalar<int>(consulta, parametros);

        // Se a quantidade de registros for maior que 0, o banco de dados existe
        return quantidadeDeRegistros > 0;
    }




}
