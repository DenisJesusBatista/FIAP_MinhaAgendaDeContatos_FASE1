using DotNet.Testcontainers.Builders;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public CustomWebApplicationFactory()
        {

        }
        public readonly PostgreSqlContainer _database = new PostgreSqlBuilder()
            .WithPortBinding(5432)
            .WithPassword("postgres")
            .Build();

        public Task InitializeAsync()
        {
            return _database.StartAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(s =>
            {
                var descriptorType =
               typeof(DbContextOptions<MinhaAgendaDeContatosContext>);

                var descriptor = s
                    .SingleOrDefault(se => se.ServiceType == descriptorType);

                if (descriptor is not null)
                {
                    s.Remove(descriptor);
                }



                s.AddDbContext<MinhaAgendaDeContatosContext>(ctx =>
                {
                    ctx.UseNpgsql(_database.GetConnectionString());
                });
                s.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>()
                .AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>()
                .AddScoped<IDDDRegiao, DDDRegiaoRepositorio>()
                .AddScoped<IContatoUpdateOnlyRepositorio, ContatoRepositorio>();
            });
            builder.UseTestServer();
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _database.StopAsync();
        }
    }

}
