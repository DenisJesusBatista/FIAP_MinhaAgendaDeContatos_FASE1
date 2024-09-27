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
    public class DeleteContatoTests : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _fixture;
        protected readonly HttpClient _client;
        public DeleteContatoTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public async Task DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Mensagem_Correta()
        {
            //Arrange
            var email = "moses.runte27@yahoo.com";

            await _fixture.CleanUpDatabase();

            await _fixture.InsertOneAsync();

            //Act
            var response = await _client.DeleteAsync($"https://localhost:7196/api/contato/{email}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            Thread.Sleep(5000);

            var getResult = await _fixture.GetByEmail(email);
            getResult.Should().NotContain(x => x.Email.Equals(email));
        }
    }
}