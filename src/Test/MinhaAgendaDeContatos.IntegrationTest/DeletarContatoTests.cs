﻿using AutoBogus;
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
using Ductus.FluentDocker.Common;
using MinhaAgendaDeContatos.Comunicacao.Resposta;



namespace MinhaAgendaDeContatos.IntegrationTest
{
    public class DeletarContatoTests : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly CustomWebApplicationFactory _fixture;
        protected readonly HttpClient _client;

        public DeletarContatoTests(CustomWebApplicationFactory fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }

        [Fact]
        public async Task DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Id_Correto()
        {
            // Arrange - Criação do Contato
            await _fixture.CleanUpDatabase();

            await _fixture.InsertOneAsync();

            // Act - Deleção do Contato

            var existingContact = await _client.GetAsync("https://localhost:7196/api/contato");
            var json = await existingContact.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RespostaContatoJson>(json);
            var existingEmail = response?.Contatos?.FirstOrDefault()?.Email;
            
            var deleteResponse = await _client.DeleteAsync($"https://localhost:7196/api/contato/{existingEmail}");

            var existingId = response?.Contatos?.FirstOrDefault()?.Id;
            var getAfterDeletion = await _client.GetAsync($"https://localhost:7196/api/contato/{existingId}");

            // Assert - Verificação da Deleção
            var jsonResponse = await deleteResponse.Content.ReadAsStringAsync();
            jsonResponse.Should().Be(ResponseMessages.ContatoApagado);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var afterDeletionJson = await getAfterDeletion.Content.ReadAsStringAsync();
            var afterDeletionResponse = JsonConvert.DeserializeObject<RespostaContatoJson>(afterDeletionJson);
            afterDeletionResponse?.Contatos.Count().Should().Be(0);
        }
    }


}
