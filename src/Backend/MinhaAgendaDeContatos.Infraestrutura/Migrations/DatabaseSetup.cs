using Microsoft.Extensions.Configuration;
using MinhaAgendaDeContatos.Infraestrutura.Migrations;
using Microsoft.AspNetCore.Builder;


public static class DatabaseSetup
{
    public static void AtualizarBaseDeDados(IConfiguration configuration, IApplicationBuilder app)
    {
        DatabaseInitializer.AtualizarBaseDeDados(configuration, app);
    }

}

