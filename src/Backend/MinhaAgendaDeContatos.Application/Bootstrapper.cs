using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Application.UseCases.DDDRegiao.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;

namespace MinhaAgendaDeContatos.Application;
public static class Bootstrapper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AdicionarUseCases(services);

    }

    /*Registrar nas configurações de dependência.*/
    private static void AdicionarUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegistrarContatoUseCase, RegistrarContatoUseCase>()
            .AddScoped<IRecuperarPorPrefixoUseCase, RecuperarPorPrefixoUseCase>()
            .AddScoped<IRecuperarDDDRegiaoPorPrefixoUseCase, RecuperarDDDRegiaoPorPrefixoUseCase>()
            .AddScoped<IRecuperarPorIdUseCase, RecuperarPorIdUseCase>() 
            .AddScoped<IRecuperarTodosContatosUseCase, RecuperarTodosContatosUseCase>()
            .AddScoped<IUpdateContatoUseCase, UpdateContatoUseCase>()                      
            .AddScoped<IDeletarContatoUseCase, DeletarContatoUseCase>();

    }
}
