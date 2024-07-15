using DotNet.Testcontainers.Builders;
using Ductus.FluentDocker.Services;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinhaAgendaDeContatos.Api;
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
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Npgsql;
using System.Diagnostics;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using global::MinhaAgendaDeContatos.Infraestrutura.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;
using DotNet.Testcontainers.Configurations;
using Testcontainers;
using Microsoft.AspNetCore;
using DotNet.Testcontainers;
using DotNet.Testcontainers.Builders;
//using DotNet.Testcontainers.Containers.Configurations.Databases;
//using DotNet.Testcontainers.Containers.Modules.Databases;
//using DotNet.Testcontainers.Networks.Builders;
//using DotNet.Testcontainers.Networks.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Hosting;
//using TestContainers.Container.Abstractions.Networks;
//using TestContainers.Container.Abstractions.Networks.Builders;
//using TestContainers.Container.Abstractions.Networks.Builders.Concrete;
//using TestContainers.Container.Database.PostgreSql;
//using TestContainers.Container.Database.PostgreSql.Builders;


namespace MinhaAgendaDeContatos.IntegrationTest
{
    #region Comentando


    //public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    //{
    //    private ICompositeService _services;
    //    private readonly PostgreSqlContainer _container;

    //    public CustomWebApplicationFactory()
    //    {
    //        _services = new Builder()
    //            .UseContainer()
    //            .UseCompose()
    //            .FromFile("docker-compose.yml")
    //            .RemoveOrphans()
    //            .Build().Start();
    //    }




    //    #region InitializeAsync
    //    public async Task InitializeAsync()
    //    {

    //        var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
    //        var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";
    //        //var masterConnectionString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=postgres;";


    //        // Wait until PostgreSQL is ready to accept connections
    //        for (int i = 0; i < 10; i++)
    //        {
    //            try
    //            {
    //                using var conn = new NpgsqlConnection(connectionString);
    //                await conn.OpenAsync();
    //                return;
    //            }
    //            catch (NpgsqlException ex)
    //            {
    //                Console.WriteLine($"Tentativa {i + 1}: Falha ao conectar ao PostgreSQL. Erro: {ex.Message}");
    //                await Task.Delay(3000); // Aguarde por 3 segundos antes de tentar novamente
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine($"Tentativa {i + 1}: Erro inesperado: {ex.Message}");
    //                await Task.Delay(3000); // Aguarde por 3 segundos antes de tentar novamente
    //            }
    //        }
    //        using (var scope = Services.CreateScope())
    //        {
    //            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    //            var app = scope.ServiceProvider.GetRequiredService<IApplicationBuilder>();

    //            // Atualiza o banco de dados e executa migrações
    //            DatabaseSetup.AtualizarBaseDeDados(configuration, app);
    //        }

    //    }
    //    #endregion

    //    #region ConfigureWebHost
    //    protected override void ConfigureWebHost(IWebHostBuilder builder)
    //    {
    //        builder.ConfigureServices(services =>
    //        {
    //            var descriptorType = typeof(DbContextOptions<MinhaAgendaDeContatosContext>);
    //            var descriptor = services.SingleOrDefault(se => se.ServiceType == descriptorType);
    //            string connectionString = "Host=localhost;Port=5432;Database=minhaagenda;Username=postgres;Password=postgres;Timeout=60;";

    //            if (descriptor != null)
    //            {
    //                services.Remove(descriptor);
    //            }

    //            // Verifica se _services não é nulo antes de usar
    //            if (_services != null)
    //            {
    //                services.AddDbContext<MinhaAgendaDeContatosContext>(ctx =>
    //                {
    //                    //ctx.UseNpgsql(connectionString);
    //                    ctx.UseNpgsql(connectionString);
    //                });
    //            }
    //            else
    //            {
    //                throw new Exception("_services não foi inicializado corretamente.");
    //            }

    //            services.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>()
    //                    .AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>()
    //                    .AddScoped<IDDDRegiao, DDDRegiaoRepositorio>()
    //                    .AddScoped<IContatoUpdateOnlyRepositorio, ContatoRepositorio>();
    //        });
    //    }
    //    #endregion

    //    #region DisposeAsync
    //    public async Task DisposeAsync()
    //    {
    //        LimparConteineres();
    //        _services.Dispose();
    //        Task.CompletedTask.Wait();
    //    }
    //    #endregion



    //    #region LimparConteineres
    //    public async Task LimparConteineres()
    //    {
    //        await ExecutarComandoLimparAsync("docker-compose", "down -v --remove-orphans");
    //        await ExecutarComandoLimparAsync("docker", "container rm -f net70-postgres-1");
    //    }
    //    #endregion

    //    #region IniciarConteineres
    //    public async Task IniciarConteineres()
    //    {
    //        // Inicia os contêineres Docker em background
    //        await ExecutarComandoLimparAsync("docker-compose", "up -d");

    //        var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";

    //    }
    //    #endregion


    //    #region ExecutarComandoLimparAsync
    //    private async Task ExecutarComandoLimparAsync(string comando, string argumentos)
    //    {
    //        var processInfo = new ProcessStartInfo
    //        {
    //            FileName = comando,
    //            Arguments = argumentos,
    //            RedirectStandardOutput = true,
    //            RedirectStandardError = true,
    //            UseShellExecute = false,
    //            CreateNoWindow = true
    //        };

    //        using var process = new Process
    //        {
    //            StartInfo = processInfo
    //        };

    //        process.Start();

    //        await process.WaitForExitAsync();

    //        if (process.ExitCode != 0)
    //        {
    //            var errorMessage = await process.StandardError.ReadToEndAsync();
    //            throw new Exception($"Erro ao executar o comando: {comando} {argumentos}. Erro: {errorMessage}");
    //        }
    //    }
    //    #endregion

    //}

    //#endregion


    //#region IniciarConteineres
    ////public async Task IniciarConteineres()
    ////{
    ////    await ExecutarComandoAsync("docker-compose", "up -d");

    ////    var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";

    ////    // Wait until PostgreSQL is ready to accept connections
    ////    for (int i = 0; i < 10; i++)
    ////    {
    ////        try
    ////        {
    ////            using var conn = new NpgsqlConnection(connectionString);
    ////            await conn.OpenAsync();
    ////            return;
    ////        }
    ////        catch (NpgsqlException ex)
    ////        {
    ////            Console.WriteLine($"Tentativa {i + 1}: Falha ao conectar ao PostgreSQL. Erro: {ex.Message}");
    ////            await Task.Delay(3000); // Aguarde por 3 segundos antes de tentar novamente
    ////        }
    ////        catch (Exception ex)
    ////        {
    ////            Console.WriteLine($"Tentativa {i + 1}: Erro inesperado: {ex.Message}");
    ////            await Task.Delay(3000); // Aguarde por 3 segundos antes de tentar novamente
    ////        }
    ////    }
    ////}
    //#endregion


    //#region ExecutarComandoAsync
    ////private async Task ExecutarComandoAsync(string comando, string argumentos)
    ////{
    ////    var processInfo = new ProcessStartInfo
    ////    {
    ////        FileName = comando,
    ////        Arguments = argumentos,
    ////        RedirectStandardOutput = true,
    ////        RedirectStandardError = true,
    ////        UseShellExecute = false,
    ////        CreateNoWindow = true
    ////    };

    ////    using var process = new Process
    ////    {
    ////        StartInfo = processInfo
    ////    };

    ////    process.Start();
    ////    await process.WaitForExitAsync();

    ////    if (process.ExitCode != 0)
    ////    {
    ////        var errorMessage = await process.StandardError.ReadToEndAsync();
    ////        throw new Exception($"Erro ao executar o comando: {comando} {argumentos}. Erro: {errorMessage}");
    ////    }
    ////}
    //#endregion


    //public class DockerComposeHostedService : IHostedService
    //{
    //    private readonly ICompositeService _services;

    //    public DockerComposeHostedService()
    //    {
    //        _services = new Builder()
    //            .UseContainer()
    //            .UseCompose()
    //            .FromFile("docker-compose.yml")
    //            .RemoveOrphans()
    //            .Build();
    //    }

    //    public async Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        await Task.Run(() => _services.Start(), cancellationToken);
    //    }

    //    public async Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        await Task.Run(() => _services.Dispose(), cancellationToken);
    //    }
    //}

    #endregion


    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private ICompositeService _services;

        public CustomWebApplicationFactory()
        {
            _services = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile("docker-compose.yml")
                .RemoveOrphans()
                .Build().Start();
        }

        public async Task InitializeAsync()
        {
            var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";

            // Wait until PostgreSQL is ready to accept connections
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    using var conn = new NpgsqlConnection(connectionString);
                    await conn.OpenAsync();
                    return;
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine($"Tentativa {i + 1}: Falha ao conectar ao PostgreSQL. Erro: {ex.Message}");
                    await Task.Delay(3000); // Aguarde por 3 segundos antes de tentar novamente
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Tentativa {i + 1}: Erro inesperado: {ex.Message}");
                    await Task.Delay(3000); // Aguarde por 3 segundos antes de tentar novamente
                }
            }

            using (var scope = Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var app = scope.ServiceProvider.GetRequiredService<IApplicationBuilder>();

                // Atualiza o banco de dados e executa migrações
                DatabaseSetup.AtualizarBaseDeDados(configuration, app);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptorType = typeof(DbContextOptions<MinhaAgendaDeContatosContext>);
                var descriptor = services.SingleOrDefault(se => se.ServiceType == descriptorType);
                string connectionString = "Host=localhost;Port=5432;Database=minhaagenda;Username=postgres;Password=postgres;Timeout=60;";

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                if (_services != null)
                {
                    services.AddDbContext<MinhaAgendaDeContatosContext>(ctx =>
                    {
                        ctx.UseNpgsql(connectionString);
                    });
                }
                else
                {
                    throw new Exception("_services não foi inicializado corretamente.");
                }

                services.AddScoped<IContatoWriteOnlyRepositorio, ContatoRepositorio>()
                        .AddScoped<IContatoReadOnlyRepositorio, ContatoRepositorio>()
                        .AddScoped<IDDDRegiao, DDDRegiaoRepositorio>()
                        .AddScoped<IContatoUpdateOnlyRepositorio, ContatoRepositorio>();
            });
        }

        public async Task DisposeAsync()
        {
            await LimparConteineres();
            _services.Dispose();
        }

        public async Task LimparConteineres()
        {
            await ExecutarComandoAsync("docker-compose", "down -v --remove-orphans");
            await ExecutarComandoAsync("docker", "container rm -f net70-postgres-1");
        }

        public async Task IniciarConteineres()
        {
            await ExecutarComandoAsync("docker-compose", "up -d");

            var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";
        }

        private async Task ExecutarComandoAsync(string comando, string argumentos)
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
    }

}

