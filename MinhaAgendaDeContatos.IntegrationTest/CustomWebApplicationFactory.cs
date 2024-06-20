using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private PostgreSQLFakeDatabase _fixture;
        public CustomWebApplicationFactory(PostgreSQLFakeDatabase fixture)
        {
            _fixture = fixture;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var teste = new List<KeyValuePair<string, string?>>()
            {
                new KeyValuePair<string, string?>("ConnectionStrings:Database", _fixture._database.GetConnectionString())
            };
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config
                .AddInMemoryCollection(teste);
            });
            builder.ConfigureServices(s =>
            {
                s.AddDbContext<MinhaAgendaDeContatosContext>(ctx =>
                {
                    ctx.UseNpgsql(_fixture._database.GetConnectionString());
                });
            });
            builder.UseTestServer();
        }
    }

    public class PostgreSQLFakeDatabase : IAsyncLifetime
    {
        public readonly PostgreSqlContainer _database = new PostgreSqlBuilder()
            .WithImage("testcontainers/helloworld:1.1.0")
            .WithPortBinding(8080, true)
            .WithExposedPort(5432)
            //.WithEnvironment(new Dictionary<string, string>()
            //{
            //    {"POSTGRES_USER", "postgres"},
            //    {"POSTGRES_PASSWORD", "postgres"}
            //})
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
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
