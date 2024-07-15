using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MinhaAgendaDeContatos.Domain.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations;
public static class DatabaseInitializer
{
    public static void AtualizarBaseDeDados(IConfiguration configuration, IApplicationBuilder app)
    {
        var conexao = configuration.GetConexao();
        var nomeDatabase = configuration.GetNomeDataBase();

        // Verifica se o banco de dados existe
        bool bancoExiste = Database.VerificarExistenciaDatabase(conexao, nomeDatabase);

        if (bancoExiste)
        {
            app.MigrateBancoDados();
        }
        else
        {
            Database.CriarDatabase(conexao, nomeDatabase);
            app.MigrateBancoDados();
        }
    }
}

