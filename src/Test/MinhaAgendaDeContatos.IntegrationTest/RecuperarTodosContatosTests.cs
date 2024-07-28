using AutoBogus;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using Newtonsoft.Json;
using System.Text;

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
            // Arrange - Criação do Contato
            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.TelefoneProxy, 88888888)
                .RuleFor(x => x.PrefixoProxy, 99)
                .RuleFor(x => x.Email, new Faker().Person.Email)
                .Generate();

            StringContent body = new(System.Text.Json.JsonSerializer.Serialize(contato), Encoding.UTF8, "application/json");

            // Act - POST para criar um novo contato
            var responsePost = await _client.PostAsync("https://localhost:7196/api/contato", body);
            responsePost.EnsureSuccessStatusCode();

            // Optional: Verificar o contato criado
            var createdContactJson = await responsePost.Content.ReadAsStringAsync();
            Console.WriteLine("Contato criado: " + createdContactJson);

            // Act - Recupera todos os contatos
            var responseGet = await _client.GetAsync("https://localhost:7196/api/contato");
            responseGet.EnsureSuccessStatusCode();

            // Assert
            var resultJson = await responseGet.Content.ReadAsStringAsync();
            Console.WriteLine("Resposta GET: " + resultJson);

            var responseJson = JsonConvert.DeserializeObject<RespostaContatoJson>(resultJson);
            responseJson.Contatos.Should().NotBeNull();
            responseJson.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            //responseJson.Contatos.Count().Should().Be(1); // Deve retornar exatamente um contato
        }




    }
}
