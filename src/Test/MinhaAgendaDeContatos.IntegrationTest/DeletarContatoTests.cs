using AutoBogus;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Api.Response;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
using Newtonsoft.Json;
//using Org.BouncyCastle.Ocsp;
using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using MinhaAgendaDeContatos.IntegrationTest;
using Microsoft.AspNetCore.Hosting;
using MinhaAgendaDeContatos.Comunicacao.Resposta;



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
        public async Task DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Id_Correto()
        {
            // Arrange - Criação do Contato
            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.TelefoneProxy, 88888888)
                .RuleFor(x => x.PrefixoProxy, 99)
                .RuleFor(x => x.Email, new Faker().Person.Email)
                .Generate();

            await _fixture.CleanUpDatabase();

            await _fixture.InsertOneAsync();          

            //Act
            var result = await _client.GetAsync("https://localhost:7196/api/contato");




            // Act - Deleção do Contato
            var deleteResponse = await _client.DeleteAsync($"https://localhost:7196/api/contato/{contato.Email}");

            
            await _fixture.CleanUpDatabase();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            response.Contatos.Should().BeAssignableTo<List<ContatoJson>>();
            response.Contatos.Count().Should().Be(1);

        }
    }


}
