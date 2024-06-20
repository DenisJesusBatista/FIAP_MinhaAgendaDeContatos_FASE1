using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string _connectionString;
        public CustomWebApplicationFactory(PostgreSQLFakeDatabase fixture)
        {
            _connectionString = fixture._database.GetConnectionString();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(s =>
            {
                s.AddDbContext<MinhaAgendaDeContatosContext>(ctx =>
                {
                    ctx.UseNpgsql(_connectionString);
                });
            });
        }
    }

    public class PostgreSQLFakeDatabase : IAsyncLifetime
    {
        public readonly PostgreSqlContainer _database = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithPortBinding(5432)
            .WithExposedPort(5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .WithCleanUp(true)
            .Build();
        public Task DisposeAsync()
        {
            return _database.DisposeAsync().AsTask();
        }

        public Task InitializeAsync()
        {
            return _database.StartAsync();
        }
    }
}
