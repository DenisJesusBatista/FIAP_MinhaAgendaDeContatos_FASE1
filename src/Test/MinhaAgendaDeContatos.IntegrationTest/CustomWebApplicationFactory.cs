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
            var command = @"
                                    BEGIN;
                                    CREATE TABLE IF NOT EXISTS public.""Contatos""
                                    (
                                        ""Id"" SERIAL PRIMARY KEY,
                                        ""DataCriacao"" timestamp without time zone NOT NULL,
                                        ""Nome"" character varying(100)  NOT NULL,
                                        ""Email"" text  NOT NULL,
                                        ""Telefone"" character varying(14)  NOT NULL,
                                        ""Prefixo"" character varying(14)  NOT NULL
                                    );
                                    CREATE TABLE IF NOT EXISTS public.""DDDRegiao""
                                    (
                                        ""Id"" SERIAL PRIMARY KEY,
                                        ""DataCriacao"" timestamp without time zone NOT NULL,
                                        ""Prefixo"" text COLLATE pg_catalog.""default"" NOT NULL,
                                        ""Estado"" text COLLATE pg_catalog.""default"" NOT NULL,
                                        ""Regiao"" text COLLATE pg_catalog.""default"" NOT NULL
                                    );
                                    END;";
            conn.Execute(command);
        }

        #region CleanUpDatabase

        public async Task CleanUpDatabase()
        {

            var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
            var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";


            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            var command = @"BEGIN;
                            DELETE FROM public.""Contatos"";
                            DELETE FROM public.""DDDRegiao"";
                            END;";

            await conn.ExecuteAsync(command);
        }

        #endregion



        public async Task CleanUpDatabase(string emailToDelete = null)
        {
            var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
            var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";

            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            var command = @"
        BEGIN;";

            if (emailToDelete != null)
            {
                command += @"
            DELETE FROM public.""Contatos"" WHERE ""Email"" = @EmailToDelete;";
            }

            command += @"
        DELETE FROM public.""DDDRegiao"";
        END;";

            if (emailToDelete != null)
            {
                await conn.ExecuteAsync(command, new { EmailToDelete = emailToDelete });
            }
            else
            {
                await conn.ExecuteAsync(command);
            }
        }


        public async Task InsertOneAsync()
        {
            var dbService = _services.Services.FirstOrDefault(s => s.Name == "/postgres");
            var connectionString = "Server=localhost;Port=5432;Database=minhaagenda;User Id=postgres;Password=postgres;";


            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            var command = @"	
                                BEGIN;
                                INSERT INTO public.""Contatos""(
	                            ""DataCriacao"", ""Nome"", ""Email"", ""Telefone"", ""Prefixo"")
	                            VALUES (
                                        current_timestamp, 
                                        'incremental', 
                                        'moses.runte27@yahoo.com',
                                        88888888, 
                                        99
                                );
                                INSERT INTO public.""DDDRegiao""(
	                            ""DataCriacao"", ""Prefixo"", ""Estado"", ""Regiao"")
	                            VALUES (current_timestamp, 99, 'CE', 'NE');
                                END;";


            await conn.ExecuteAsync(command);
        }

        public override ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return base.DisposeAsync();
        }

    }
}