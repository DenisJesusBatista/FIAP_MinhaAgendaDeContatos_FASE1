﻿using AutoBogus;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using Newtonsoft.Json;
using System.Net;
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
            var response = await _client.PostAsync("https://localhost:7196/api/contato", body);

            // Verifica se o POST foi bem-sucedido
            response.EnsureSuccessStatusCode();


            // Act - Recupera todos os contatos
            var result = await _client.GetAsync("https://localhost:7196/api/contato");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verifica a resposta
            var json = await result.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            //response.Contatos.Should().NotBeNull();
            //response.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            //response.Contatos.Count().Should().Be(1); // Deve retornar exatamente um contato

            //_fixture.LimparConteineres();
        }


    }
}
