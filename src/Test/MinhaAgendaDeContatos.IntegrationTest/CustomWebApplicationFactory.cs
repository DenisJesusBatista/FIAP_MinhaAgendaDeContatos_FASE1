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

namespace MinhaAgendaDeContatos.IntegrationTest
{

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        private ICompositeService _services;

        public CustomWebApplicationFactory()
        {
            _services = new Builder()
               .UseContainer()
               .UseCompose()
               .FromFile("docker-compose.yml")
               .RemoveOrphans()
               .WaitForPort("postgres", "5432/tcp", 30000)
               .NoRecreate()
               .Build().Start();

            Thread.Sleep(2000);

            var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
            var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";


            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            #region Comentado a criação das tabelas

            //var command = @"
            //                        BEGIN;
            //                        CREATE TABLE IF NOT EXISTS public.""Contatos""
            //                        (
            //                            ""Id"" SERIAL PRIMARY KEY,
            //                            ""DataCriacao"" timestamp without time zone NOT NULL,
            //                            ""Nome"" character varying(100)  NOT NULL,
            //                            ""Email"" text  NOT NULL,
            //                            ""Telefone"" character varying(14)  NOT NULL,
            //                            ""Prefixo"" character varying(14)  NOT NULL
            //                        );
            //                        CREATE TABLE IF NOT EXISTS public.""DDDRegiao""
            //                        (
            //                            ""Id"" SERIAL PRIMARY KEY,
            //                            ""DataCriacao"" timestamp without time zone NOT NULL,
            //                            ""Prefixo"" text COLLATE pg_catalog.""default"" NOT NULL,
            //                            ""Estado"" text COLLATE pg_catalog.""default"" NOT NULL,
            //                            ""Regiao"" text COLLATE pg_catalog.""default"" NOT NULL
            //                        );
            //                        END;";
            //conn.Execute(command);

            #endregion

        }

        #region CleanUpDatabase

        //public async Task CleanUpDatabase()
        //{

        //    var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
        //    var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";


        //    using var conn = new NpgsqlConnection(connectionString);
        //    await conn.OpenAsync();

        //    var command = @"BEGIN;
        //                    DELETE FROM public.""Contatos"";
        //                    DELETE FROM public.""DDDRegiao"";
        //                    END;";

        //    await conn.ExecuteAsync(command);
        //}

        #endregion

        #region InsertOneAsync

        //public async Task InsertOneAsync()
        //{
        //    var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
        //    var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";


        //    using var conn = new NpgsqlConnection(connectionString);
        //    await conn.OpenAsync();

        //    var command = @"	
        //                        BEGIN;
        //                        INSERT INTO public.""Contatos""(
        //                     ""DataCriacao"", ""Nome"", ""Email"", ""Telefone"", ""Prefixo"")
        //                     VALUES (
        //                                current_timestamp, 
        //                                'incremental', 
        //                                'moses.runte27@yahoo.com',
        //                                88888888, 
        //                                99
        //                        );
        //                        INSERT INTO public.""DDDRegiao""(
        //                     ""DataCriacao"", ""Prefixo"", ""Estado"", ""Regiao"")
        //                     VALUES (current_timestamp, 99, 'CE', 'NE');
        //                        END;";


        //    await conn.ExecuteAsync(command);
        //}

        #endregion


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