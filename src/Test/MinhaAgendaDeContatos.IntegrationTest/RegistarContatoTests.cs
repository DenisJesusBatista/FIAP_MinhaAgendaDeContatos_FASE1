using AutoBogus;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RegistrarContatoTests : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _fixture;
        protected readonly HttpClient _client;
        public RegistrarContatoTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public async Task RegistrarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Mensagem_Correta()
        {
            //Arrange
            var email = new Faker().Person.Email;
            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.TelefoneProxy, 88888888)
                .RuleFor(x => x.PrefixoProxy, 99)
                .RuleFor(x => x.Email, email)
                .Generate();

            StringContent body = new(JsonSerializer.Serialize(contato), Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("https://localhost:7196/api/contato", body);

            //Assert
            var jsonResponse = await response.Content.ReadAsStringAsync();
            jsonResponse.Should().Be(ResponseMessages.ContatoCriado);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Thread.Sleep(10000);

            var getResult = await _fixture.GetByEmail(email);
            getResult.Should().Contain(x => x.Email.Equals(email.ToLower()));
        }
    }
}