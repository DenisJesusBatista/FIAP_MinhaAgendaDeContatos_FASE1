using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations;
public static class MigrationExtension
{
    public static void MigrateBancoDados(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();


        runner.ListMigrations();

        runner.MigrateUp();

    }
}
