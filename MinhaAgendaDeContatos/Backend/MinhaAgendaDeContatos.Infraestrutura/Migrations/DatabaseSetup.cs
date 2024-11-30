using FluentMigrator.Runner.Generators.Postgres;
using FluentMigrator.Runner.Processors.Postgres;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
using Npgsql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;


public static class DatabaseSetup
{
    public static void AtualizarBaseDeDados(IConfiguration configuration, IApplicationBuilder app)
    {
        DatabaseInitializer.AtualizarBaseDeDados(configuration, app);
    }

}

