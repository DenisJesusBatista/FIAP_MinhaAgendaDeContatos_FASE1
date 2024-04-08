using FluentMigrator.Runner;
using MinhaAgendaDeContatos.Domain.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using Microsoft.EntityFrameworkCore;

namespace MinhaAgendaDeContatos.Infraestrutura;
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
        //var versaoServidor = new PostGressVersion(new Version(7, 0, 26));

        var connectionString = configurationManager.GetConexaoCompleta();

        
        services.AddDbContext<MinhaAgendaDeContatosContext>(dbCobtextoOpcoes =>
        {
            dbCobtextoOpcoes.UseNpgsql(connectionString);

        });

        //services.AddDbContext<MinhaAgendaDeContatosContext>(options =>
        //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        

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
         .AddScoped<IContatoUpdateOnlyRepositorio, ContatoRepositorio>();


    }
}
