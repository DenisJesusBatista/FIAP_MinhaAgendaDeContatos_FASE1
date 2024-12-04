using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations;
public static class MigrationExtension
{
    public static async Task MigrateBancoDadosAsync(this IApplicationBuilder app)
    {
        await Task.Delay(10000); // Atraso de 10 segundos

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            // Executar suas operações de migração aqui
            runner.ListMigrations();
            runner.MigrateUp();
        }

    }
}
