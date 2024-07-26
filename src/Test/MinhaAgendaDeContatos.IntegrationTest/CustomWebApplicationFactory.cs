using DotNet.Testcontainers.Builders;
using Ductus.FluentDocker.Services;
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
using Testcontainers.PostgreSql;
using Ductus.FluentDocker.Builders;
using Npgsql;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Dapper;
using Xunit;
using Docker.DotNet.Models;
using Docker.DotNet;
using Ductus.FluentDocker.Commands;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace MinhaAgendaDeContatos.IntegrationTest
{

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        private ICompositeService _services;

        public CustomWebApplicationFactory()
        {
            StartDockerComposeIfNotRunningAsync();

        }

        public static async Task StartDockerComposeIfNotRunningAsync()
        {
            var projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName;
            var composeFilePath = Path.Combine(projectRoot, "src", "Test", "MinhaAgendaDeContatos.IntegrationTest", "docker-compose.yml");

            if (!await IsContainerRunningAsync("minhaagenda-database"))
            {
                var builder = new Builder()
                    .UseContainer()
                    .UseCompose()
                    .FromFile(composeFilePath)
                    .RemoveOrphans()
                    .WaitForPort("minhaagenda-database", "5432/tcp", 30000) // Use o nome correto do serviço
                    .NoRecreate();
                //.Build()
                //.Start();

                var compositeService = builder.Build();
                var services = compositeService.Start();

                // Aguarde o banco de dados ficar pronto
                await Task.Delay(2000); // Melhor utilizar uma abordagem mais robusta para aguardar o serviço

                var dbService = compositeService.Containers.FirstOrDefault(s => s.Name == "/minhaagenda-database"); // Procura pelo serviço usando o nome do container
                var connectionString = GetConnectionString(dbService);

                //var dbService = compositeService.Containers.FirstOrDefault(s => s.Name == "/minhaagenda-database"); // Procura pelo serviço usando o nome do container

                //var connectionString = "Host=minhaagenda-database;Port=5432;Database=minhaagenda;Username=postgres;Password=postgres;"; // Corrija o nome do serviço

                //var connectionString = GetConnectionString(dbService);

                // Aguarde o banco de dados ficar pronto
                await WaitForDatabaseReadyAsync(connectionString, 30000); // Timeout de 30 segundos


                using var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync(); // Use `OpenAsync` para operações assíncronas
            }
        }

        #region Garantir que o banco esteja pronto.
        public static async Task WaitForDatabaseReadyAsync(string connectionString, int timeoutMilliseconds)
        {
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < timeoutMilliseconds)
            {
                try
                {
                    using var conn = new NpgsqlConnection(connectionString);
                    await conn.OpenAsync();
                    // Se conseguiu conectar, o banco de dados está pronto
                    return;
                }
                catch (NpgsqlException)
                {
                    // Banco de dados ainda não está pronto, aguarde e tente novamente
                    await Task.Delay(1000); // Espera de 1 segundo antes da próxima tentativa
                }
            }

            throw new TimeoutException("O banco de dados não ficou disponível a tempo.");
        }
        #endregion

        private static string GetConnectionString(IContainerService containerService)
        {
            // Construct and return the connection string based on the service information
            return $"Host=minhaagenda-database;Port=5432;Database=minhaagenda;Username=postgres;Password=postgres;";
        }      
        private static async Task<bool> IsContainerRunningAsync(string containerName)
        {
            try
            {
                using (var client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient())
                {
                    var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                    return containers.Any(c => c.Names.Any(n => n.Contains(containerName)) && c.State == "running");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }



        public override ValueTask DisposeAsync()
        {
            LimparConteineres();
            GC.SuppressFinalize(this);
            return base.DisposeAsync();
        }

        #region LimparConteineres
        public async Task LimparConteineres()
        {
            await ExecutarComandoLimparAsync("docker-compose", "down -v --remove-orphans");
            await ExecutarComandoLimparAsync("docker", "container rm -f net70-postgres-1");
        }
        #endregion

        #region ExecutarComandoLimparAsync
        private async Task ExecutarComandoLimparAsync(string comando, string argumentos)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = comando,
                Arguments = argumentos,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process
            {
                StartInfo = processInfo
            };

            process.Start();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var errorMessage = await process.StandardError.ReadToEndAsync();
                throw new Exception($"Erro ao executar o comando: {comando} {argumentos}. Erro: {errorMessage}");
            }
        }
        #endregion

    }
}