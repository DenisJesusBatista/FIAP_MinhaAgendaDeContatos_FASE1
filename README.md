# Sobre o projeto MinhaAgendaDeContatos

Este projeto é uma API implementada em .NET Core que foi construída utilizando a arquitetura Domain Drive Design.


# Features

- Registrar contatos;
- Consultar contatos por prefixo;
- Consultar todos os contatos
- Alterar contato por Id ou Email;
- Deletar contato;


# Requisitos
  
* Visual Studio 2022
* PostGres 16.2-1 ou mais


# Instalação
  
## Pacotes ( Plugins )

### Projeto InfraEstrutura
  * Dapper
  * NpgSql (7.0.6)
  * NpgSql.EntityFrameworkCore
  * PostgresSql
  * Fluent MIgrator
  * FluentMigrator.Runner.Postgres
  * Microsoft.AspNetCore.Http.Abstractions
  * Microsoft.Extensions.Logging

### Projeto API 
  * AutoMapper.Extensions.Microsoft.DependencyInjection
  * ConfigurationManager

### Projeto Application
  * AutoMapper
  * FluentValidation

# Configuração
* Ao realizar a instalação do postgres será necessário definir o usuário e senha, conforme arquivo `appsettings.Development.json` na propriedade `ConnectionStrings`.
* Foram configurados os EndPoints `RegistrarContato`, `RecuperarPorPrefixo`, `RecuperarTodosContatos`, `Deletar` e `UpdateContato` na ContatoController.

## Configuração de Injeção de Dependência
* Configuração feita na `Program.cs`

  ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/4cacdb4e-10dd-475c-baf3-a7b356be78f8)

* Os registros da injeção de dependência para os useCase estão sendo feitas na classe `Bootstrapper.cs`, conforme imagem abaixo:

  ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/2153ec77-f4ea-4912-a712-3043ec918610)

* Registro de injeção de dependência para o banco de dados e repositório.

  ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/e486068c-1dae-487d-b428-7c2a88ef1698)

## Criação de tabela

* Criando a tabela no banco com as colunas e ColunasPadrões, arquivo `Versao0000001` que se encontra em: MinhaAgendaDeContatos.Infraestrutura\Migrations\Versoes

  ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/8452b12d-1f45-4c80-b4d1-3ae271bebe17)


 * Migrations que faz a chamada UP da criação da tabela acima.

   ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/6d6b6d07-b1f2-4638-b1ba-1f27a5823b10)

## Persistência dos dados
* Criado no projeto `MinhaAgendaDeContatos.Domain` a pasta repositorio onde consta os arquivo que serão responsavéis em deletar, alterar, inserir e excluir.

  ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/c6db1c2e-fe33-484d-a3e9-aa9aa94857b2)

* No projeto `MinhaAgendaDeContatos.Application` e na pasta UseCases consta toda a regra de negócio separadas por responsabilidade.
 
    ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/c2540ebe-6a73-4283-bcf1-0dd8afdace41)

* Exemplo do metódo de alteração
  
  ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/a2b3461a-c897-4dad-9a14-f5f645848f59)


  ## Documentação da API

  * Configuração necessária no projeto
 
    ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/fa3ffc24-ee6f-448c-a950-03f577f74fc5)


  * Realizado a documentação na API.
 
    ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/747d04aa-94ab-4d43-86a7-70d76f2170c9)

  * EndPoint
 
    ![image](https://github.com/DenisJesusBatista/FIAP_MinhaAgendaDeContatos_FASE1/assets/52789764/5c272d23-3eca-4979-bde2-bdd0d252c2eb)

  

 


   

