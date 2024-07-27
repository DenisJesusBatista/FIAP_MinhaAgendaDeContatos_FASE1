using AutoBogus;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RegistrarContatoTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _fixture;
        private readonly HttpClient _client;

        public RegistrarContatoTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
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

            // Verifica se o POST foi bem-sucedido
            response.EnsureSuccessStatusCode();

            //Assert
            var jsonResponse = await response.Content.ReadAsStringAsync();
            jsonResponse.Should().Be(ResponseMessages.ContatoCriado);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}