using AutoBogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using Newtonsoft.Json;
using System.Net;
using Testcontainers.PostgreSql;
using Xunit.Sdk;

namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RecuperarTodosContatosTests : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _fixture;
        protected readonly HttpClient _client;
        public RecuperarTodosContatosTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public async Task RecuperarTodosContatos_Quando_Sucesso_Deve_Retornar_Ok_Com_Quantidade_Correta()
        {
            //Arrange
            await _fixture.CleanUpDatabase();

            await _fixture.InsertOneAsync();

            //Act
            var result = await _client.GetAsync("https://localhost:7196/api/contato");

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            response?.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            response?.Contatos.Count().Should().Be(1);
        }

    }
}
