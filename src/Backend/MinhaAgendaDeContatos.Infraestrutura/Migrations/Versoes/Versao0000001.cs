using FluentMigrator;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Logging;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations.Versoes;

[Migration((long)NumeroVersoes.CriarTabelaContato, "Cria tabela contato")]
public class Versao0000001 : Migration
{
    private readonly ILogger<Migration> _logger;

    public Versao0000001(ILogger<Migration> logger)
    {
        _logger = logger;
        CustomLogger.Arquivo = true;
        //Up();
    }

    public override void Down()
    {
    }

    public override void Up()
    {
        CustomLogger.Arquivo = true;

      

        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Contatos"));

        tabela
            .WithColumn("Nome").AsString(100).NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Telefone").AsString(14).NotNullable()
            .WithColumn("Prefixo").AsString(14).NotNullable();

        _logger.LogInformation("Teste de Log do tipo information");

    }
}
