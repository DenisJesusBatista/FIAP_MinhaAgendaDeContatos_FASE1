## Sobre o projeto MinhaAgendaDeContatos

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
  
  ## Pacotes ( Plugin )

  ### Projeto InfraEstrutura
  * Dapper
  * NpgSql (7.0.6)
  * NpgSql.EntityFrameworkCore
  * PostgresSql
  * Fluent MIgrator
  * FluentMigrator.Runner.Postgres
  * Microsoft.AspNetCore.Http.Abstractions
  * Microsoft.Extensions.Logging

  ### Projeto API (Plugin)
  * AutoMapper.Extensions.Microsoft.DependencyInjection
  * ConfigurationManager

  ### Projeto Application
  * AutoMapper
  * FluentValidation
