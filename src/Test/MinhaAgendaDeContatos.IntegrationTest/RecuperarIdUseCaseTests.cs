using FluentAssertions;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using Newtonsoft.Json;
using System.Net;


namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RecuperarIdUseCaseTests : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _fixture;
        protected readonly HttpClient _client;

        public RecuperarIdUseCaseTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }


        [Fact]
        public async Task RecuperarContato_AposCriacao_DeveRetornar_Ok_ComContatoCorreto()
        {

            // Arrange
            await _fixture.CleanUpDatabase();

            await _fixture.InsertOneAsync();

            var result = await _client.GetAsync("https://localhost:7196/api/contato");
            var json = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RespostaContatoJson>(json);

            var createdId = response?.Contatos?.FirstOrDefault()?.Id;

            // Act - GET para recuperar o contato recém-criado
            var resultById = await _client.GetAsync($"https://localhost:7196/api/contato/{createdId}");

            // Assert
            resultById.StatusCode.Should().Be(HttpStatusCode.OK);
            var jsonString = await result.Content.ReadAsStringAsync();
            var responseById = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            responseById?.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            responseById?.Contatos.Count().Should().Be(1);
            responseById?.Contatos?.FirstOrDefault()?.Id.Should().Be(createdId);

        }
    }
}
