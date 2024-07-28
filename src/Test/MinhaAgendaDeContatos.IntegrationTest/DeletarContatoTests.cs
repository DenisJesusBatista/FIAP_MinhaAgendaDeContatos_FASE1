using AutoBogus;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using System.Net;
using System.Text;


namespace MinhaAgendaDeContatos.IntegrationTest
{    
    public class DeletarContatoTests : IClassFixture<CustomWebApplicationFactory>
    {          
        private readonly CustomWebApplicationFactory _fixture;
        private readonly HttpClient _client;

        public DeletarContatoTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public async Task DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Mensagem_Correta()
        {
            // Arrange - Criação do Contato
            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.TelefoneProxy, 88888888)
                .RuleFor(x => x.PrefixoProxy, 99)
                .RuleFor(x => x.Email, new Faker().Person.Email)
                .Generate();

            StringContent body = new(System.Text.Json.JsonSerializer.Serialize(contato), Encoding.UTF8, "application/json");

            // Act - POST para criar um novo contato            
            var response = await _client.PostAsync("https://localhost:7196/api/contato", body);

            // Verifica se o POST foi bem-sucedido
            response.EnsureSuccessStatusCode();

            // Act - Deleção do Contato
            var deleteResponse = await _client.DeleteAsync($"https://localhost:7196/api/contato/{contato.Email}");

            // Assert
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verifique o conteúdo da resposta
            var responseContent = await deleteResponse.Content.ReadAsStringAsync();
            responseContent.Should().Be("Contato Apagado Com Sucesso"); // Ajustado para verificar a mensagem específica
        }


    }


}
