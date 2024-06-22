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
    public class RegistrarContatoTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        private readonly IServiceScope _scope;
        protected readonly MinhaAgendaDeContatosContext DbContext;
        protected readonly HttpClient _client;
        public RegistrarContatoTests(CustomWebApplicationFactory factory)
        {
            _scope = factory.Services.CreateScope();

            DbContext = _scope.ServiceProvider
                .GetRequiredService<MinhaAgendaDeContatosContext>();

            _client = factory.CreateClient();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            DbContext?.Dispose();
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