using AutoBogus;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using Newtonsoft.Json;
using System.Net;
using System.Text;


namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RecuperarIdUseCaseTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public RecuperarIdUseCaseTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;

            // Atribuir _client para null
            //_client = null;

            _client = _factory.CreateClient(); // Usar o client da fábrica
        }


        //[Fact]
        public async Task RecuperarContato_AposCriacao_DeveRetornar_Ok_ComContatoCorreto()
        {
            try
            {

                #region RecuperarContato_AposCriacao_DeveRetornar_Ok_ComContatoCorreto                

                // Arrange
                var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                    .RuleFor(x => x.TelefoneProxy, 88888888)
                    .RuleFor(x => x.PrefixoProxy, 99)
                    .RuleFor(x => x.Email, new Faker().Person.Email)
                    .Generate();

                var body = System.Text.Json.JsonSerializer.Serialize(contato);
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

                // Act - POST para criar um novo contato
                var postResponse = await _client.PostAsync("https://localhost:7196/api/contato", stringContent);
                postResponse.EnsureSuccessStatusCode();

                // Extrair o ID do contato criado do banco de dados
                long createdContatoId;
                using (var scope = _factory.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
                    var contatoSalvo = await context.Contatos.FirstOrDefaultAsync(c => c.Email == contato.Email);

                    contatoSalvo.Should().NotBeNull();
                    contatoSalvo.Email.Should().Be(contato.Email); // Verifique outros atributos se necessário

                    // Salvar o ID do contato criado para uso em outros testes, se necessário
                    createdContatoId = contatoSalvo.Id;
                }

                // Act - GET para recuperar o contato recém-criado
                var getResponse = await _client.GetAsync($"https://localhost:7196/api/contato/{createdContatoId}");

                // Assert
                getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                var jsonResponse = await getResponse.Content.ReadAsStringAsync();

                // Desserializar o JSON retornado
                var respostaContato = JsonConvert.DeserializeObject<RespostaContatoJson>(jsonResponse);

                // Verificar se a resposta não é nula
                respostaContato.Should().NotBeNull();

                // Verificar se existe pelo menos um contato na resposta
                respostaContato.Contatos.Should().NotBeNull().And.NotBeEmpty();

                // Encontrar o contato específico pelo ID
                var returnedContato = respostaContato.Contatos.FirstOrDefault(c => c.Id == createdContatoId);

                // Verificar se o contato retornado não é nulo
                returnedContato.Should().NotBeNull();

                // Verificar os campos individualmente, conforme necessário
                returnedContato.Email.Should().Be(contato.Email);
                returnedContato.DDDRegiao.Prefixo.Should().Be(contato.PrefixoProxy.ToString());
                returnedContato.Telefone.Should().Be(contato.TelefoneProxy.ToString());

                #endregion


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
