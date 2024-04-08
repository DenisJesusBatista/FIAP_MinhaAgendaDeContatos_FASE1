using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Application.Servicoes.Token;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;

namespace MinhaAgendaDeContatos.Application;
public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarTokenJWT(services, configuration);
        AdicionarUseCases(services);
        //services.AddScoped<IRegistrarContatoUseCase, RegistrarContatoUseCase>();

    }


    private static void AdicionarTokenJWT(IServiceCollection services, IConfiguration configuration)
    {
        var sectionTempoDeVida = configuration.GetRequiredSection("Configuracoes:TempoVidaToken");
        var sectionKey = configuration.GetRequiredSection("Configuracoes:ChaveToken");

        services.AddScoped(option => new TokenController(int.Parse(sectionTempoDeVida.Value), sectionKey.Value));
    }

    /*Registrar nas configurações de dependência.*/
    private static void AdicionarUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegistrarContatoUseCase, RegistrarContatoUseCase>()
            .AddScoped<IRecuperarPorPrefixoUseCase, RecuperarPorPrefixoUseCase>()
            .AddScoped<IRecuperarTodosContatosUseCase, RecuperarTodosContatosUseCase>()
            .AddScoped<IUpdateContatoUseCase, UpdateContatoUseCase>()                      
            .AddScoped<IDeletarContatoUseCase, DeletarContatoUseCase>();

    }
}
