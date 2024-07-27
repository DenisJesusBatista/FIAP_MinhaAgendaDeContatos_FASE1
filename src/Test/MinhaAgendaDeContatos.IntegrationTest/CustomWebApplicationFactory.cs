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
                    .WaitForPort("minhaagenda-database", "5432/tcp", 30000) // Use o nome correto do servi�o
                    .NoRecreate();               

                var compositeService = builder.Build();
                var services = compositeService.Start();

                // Aguarde o banco de dados ficar pronto
                await Task.Delay(2000); // Espera por 2 segundos

                var dbService = compositeService.Containers.FirstOrDefault(s => s.Name == "/minhaagenda-database"); // Procura pelo servi�o usando o nome do container
                var connectionString = GetConnectionString(dbService);               

                // Aguarde o banco de dados ficar pronto
                await WaitForDatabaseReadyAsync(connectionString, 30000); // Timeout de 30 segundos


                using var conn = new NpgsqlConnection(connectionString);
                await conn.OpenAsync(); // Use `OpenAsync` para opera��es ass�ncronas
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
                    // Se conseguiu conectar, o banco de dados est� pronto
                    return;
                }
                catch (NpgsqlException)
                {
                    // Banco de dados ainda n�o est� pronto, aguarde e tente novamente
                    await Task.Delay(1000); // Espera de 1 segundo antes da pr�xima tentativa
                }
            }

            throw new TimeoutException("O banco de dados n�o ficou dispon�vel a tempo.");
        }
        #endregion

        private static string GetConnectionString(IContainerService containerService)
        {
            // Construir e retornar a string de conex�o com base nas informa��es do servi�o
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
                // Registrar a exce��o
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