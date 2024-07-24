﻿using AutoBogus;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class RecuperarIdUseCaseTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _fixture;
        private readonly HttpClient _client;


        public RecuperarIdUseCaseTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            //_client = _fixture.CreateClient();
        }


        [Fact]
        public async Task RecuperarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Quantidade_Correta()
        {
            // Arrange
            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                    .RuleFor(x => x.TelefoneProxy, 88888888)
                    .RuleFor(x => x.PrefixoProxy, 99)
                    .RuleFor(x => x.Email, new Faker().Person.Email)
                    .Generate();


            //await _fixture.CleanUpDatabase();

            //await _fixture.InsertOneAsync();

            var _client = _fixture.CreateClient();

            StringContent body = new(System.Text.Json.JsonSerializer.Serialize(contato), Encoding.UTF8, "application/json");


            // Act - POST para criar um novo contato
            //Act
            var response = await _client.PostAsync("https://localhost:7196/api/contato", body);


            // Verifica se o POST foi bem-sucedido
            response.EnsureSuccessStatusCode();


            //Act
            var result = await _client.GetAsync("https://localhost:7196/api/contato");


            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await result.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            //response.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            //response.Contatos.Count().Should().Be(1);
        }
    }
}
