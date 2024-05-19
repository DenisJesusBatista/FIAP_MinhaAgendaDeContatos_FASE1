using FluentMigrator.Builders.Create.Table;
using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations;
[ExcludeFromCodeCoverage]
public static class VersaoBase
{
    public static ICreateTableColumnOptionOrWithColumnSyntax InserirColunasPadrao(ICreateTableWithColumnOrSchemaOrDescriptionSyntax tabela)
    {
        return tabela
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("DataCriacao").AsDateTime().NotNullable();

    }
}
