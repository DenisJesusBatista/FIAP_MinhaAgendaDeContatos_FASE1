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
    public class RecuperarIdUseCaseTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _fixture;
        private readonly HttpClient _client;


        public RecuperarIdUseCaseTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }


        [Fact]
        public async Task RecuperarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Quantidade_Correta()
        {
            // Arrange
            var contato = new AutoFaker<Contato>()
                .RuleFor(x => x.Email, "aleescossio@hotmail.com")
                .RuleFor(x => x.DataCriacao, DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc))
                .RuleFor(x => x.Telefone, "99999999")
                .RuleFor(x => x.Prefixo, "66")
            .Generate();


            await _fixture.CleanUpDatabase();

            await _fixture.InsertOneAsync();

            //Act
            var result = await _client.GetAsync("https://localhost:7196/api/contato");


            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            response.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            response.Contatos.Count().Should().Be(1);
        }
    }
}
