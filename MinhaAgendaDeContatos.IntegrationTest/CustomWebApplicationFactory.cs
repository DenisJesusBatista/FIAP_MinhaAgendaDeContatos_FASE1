using DotNet.Testcontainers.Builders;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var configuration  = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Appsettings.Development.json").Build();

            var connectionString = configuration.GetSection("ConnectionStrings:Conexao").Value.Split(new[] { '=', ';' });

            var password = connectionString[connectionString.Length - 2];

            _container = new PostgreSqlBuilder()
                            .WithPortBinding(5432)
                            .WithDatabase("minhaagenda")
                            .WithPassword(password)
                            .Build();
        }

        private readonly PostgreSqlContainer _container;

        public Task InitializeAsync()
        {
            return _container.StartAsync();
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
                    ctx.UseNpgsql(_container.GetConnectionString());
                });
                s.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>()
                .AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>()
                .AddScoped<IDDDRegiao, DDDRegiaoRepositorio>()
                .AddScoped<IContatoUpdateOnlyRepositorio, ContatoRepositorio>();
            });
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            return _container.StopAsync();
        }
    }

}
