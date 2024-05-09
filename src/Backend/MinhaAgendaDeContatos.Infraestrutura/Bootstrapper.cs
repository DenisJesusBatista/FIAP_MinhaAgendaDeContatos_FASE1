#region Pluglins
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Domain.Extension;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using System.Reflection;

namespace MinhaAgendaDeContatos.Infraestrutura;

#endregion
public static class Bootstrapper
{
    public static void AddRepositorio(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigratorPostgres(services, configurationManager);
        AddUnidadeDeTrabalho(services);
        AddRepositorios(services);
        AddContexto(services, configurationManager);
    }

    private static void AddUnidadeDeTrabalho(this IServiceCollection services)
    {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
    }

    private static void AddContexto(IServiceCollection services, IConfiguration configurationManager)
    {
        var connectionString = configurationManager.GetConexaoCompleta();

        services.AddDbContext<MinhaAgendaDeContatosContext>(dbCobtextoOpcoes =>
        {
            dbCobtextoOpcoes.UseNpgsql(connectionString);
        });
    }
    private static void AddFluentMigratorPostgres(IServiceCollection services, IConfiguration configurationManager)
    {
        services.AddFluentMigratorCore().ConfigureRunner(c =>
        c.AddPostgres()
        .WithGlobalConnectionString(configurationManager.GetConexaoCompleta()).ScanIn(Assembly.Load("MinhaAgendaDeContatos.Infraestrutura")).For.All());
    }

    private static void AddRepositorios(IServiceCollection services)
    {
        services.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>()
            .AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>()
            .AddScoped<IDDDRegiao, DDDRegiaoRepositorio>()
         .AddScoped<IContatoUpdateOnlyRepositorio, ContatoRepositorio>();
    }
}
