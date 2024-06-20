using AutoBogus;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RegistrarContatoTests : IClassFixture<PostgreSQLFakeDatabase>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        public RegistrarContatoTests(PostgreSQLFakeDatabase factory)
        {
            var clientOptions = new WebApplicationFactoryClientOptions();
            clientOptions.AllowAutoRedirect = false;

            _factory = new CustomWebApplicationFactory(factory);
            _client = _factory.CreateClient(clientOptions);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _factory.Dispose();
        }

        [Fact]
        public async Task RegistrarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Mensagem_Correta()
        {
            //Arrange
            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.TelefoneProxy, 88888888)
                .RuleFor(x => x.PrefixoProxy, 99)
                .RuleFor(x => x.Email, new Faker().Person.Email)
                .Generate();

            StringContent body = new(JsonSerializer.Serialize(contato), Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("https://localhost:7196/api/contato", body);

            //Assert
            var jsonResponse = await response.Content.ReadAsStringAsync();
            jsonResponse.Should().Be(ResponseMessages.ContatoCriado); 
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}