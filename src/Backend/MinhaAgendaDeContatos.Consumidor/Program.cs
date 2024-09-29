using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Consumidor;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MinhaAgendaDeContatos.Domain.Extension;
using System.Threading.Tasks;
using AutoMapper;
using MinhaAgendaDeContatos.Produtor.RabbitMqSettings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;

class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        // Testar a conexão com o banco de dados
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
            //var testeConexao = new TesteConexao(context);
            //await testeConexao.TestarConexao();
        }

        // Executar o host
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
           ;
}
