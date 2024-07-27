using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Ductus.FluentDocker.Builders;
using Npgsql;
using System.Diagnostics;
using Docker.DotNet.Models;
using Docker.DotNet;

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

                var compositeService = builder.Build();
                var services = compositeService.Start();

                // Aguarde o banco de dados ficar pronto
                await Task.Delay(2000); // Espera por 2 segundos

                var dbService = compositeService.Containers.FirstOrDefault(s => s.Name == "/minhaagenda-database"); // Procura pelo serviço usando o nome do container
                var connectionString = GetConnectionString(dbService);               

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
            // Construir e retornar a string de conexão com base nas informações do serviço
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
                // Registrar a exceção
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public override ValueTask DisposeAsync()
        {            
            GC.SuppressFinalize(this);
            return base.DisposeAsync();
        }       

    }
}