//using AutoBogus;
//using Bogus;
//using Bogus.Extensions;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using MinhaAgendaDeContatos.Api.Response;
//using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
//using MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
//using Newtonsoft.Json;
////using Org.BouncyCastle.Ocsp;
//using System;
//using System.Net;
//using System.Text;
//using Newtonsoft.Json;
//using MinhaAgendaDeContatos.IntegrationTest;
//using Microsoft.AspNetCore.Hosting;



//namespace MinhaAgendaDeContatos.IntegrationTest
//{
//    #region Comentado
//    //public class DeletarContatoTests : IClassFixture<CustomWebApplicationFactory>
//    //{
//    //    private readonly HttpClient _client;
//    //    private readonly IServiceProvider _serviceProvider;
//    //    private readonly CustomWebApplicationFactory _factory;

//    //    public DeletarContatoTests(CustomWebApplicationFactoryWebApplicationFactory<TStartup> factory)
//    //    {
//    //        _factory = factory;

//    //        // Atribuir _client para null
//    //        //_client = null;

//    //        _client = factory.CreateClient();
//    //        _serviceProvider = factory.Services;
//    //    }

//    //    //[Fact]
//    //    public async Task DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Id_Correto()
//    //    {
//    //        try
//    //        {
//    //            // Arrange - Criação do Contato                

//    //            var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
//    //                .RuleFor(x => x.TelefoneProxy, 88888888)
//    //                .RuleFor(x => x.PrefixoProxy, 99)
//    //                .RuleFor(x => x.Email, new Faker().Person.Email)
//    //                .Generate();

//    //            // Serializa o objeto contato para JSON
//    //            var body = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

//    //            var createResponse = await _client.PostAsync("https://localhost:7196/api/contato", body);

//    //            if (createResponse.StatusCode != HttpStatusCode.OK)
//    //            {
//    //                var errorMessage = await createResponse.Content.ReadAsStringAsync();
//    //                throw new Exception($"Erro ao criar contato: {errorMessage}");
//    //            }

//    //            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
//    //            var jsonResponse = await createResponse.Content.ReadAsStringAsync();
//    //            jsonResponse.Should().Be(ResponseMessages.ContatoCriado);

//    //            // Extrair o ID do contato criado a partir da resposta
//    //            //var createdContatoId = JsonConvert.DeserializeObject<long>(jsonResponse);

//    //            // Extrair o ID do contato criado do banco de dados
//    //            long createdContatoId;
//    //            using (var scope = _serviceProvider.CreateScope())
//    //            {
//    //                var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
//    //                var contatoSalvo = await context.Contatos.FirstOrDefaultAsync(c => c.Email == contato.Email);

//    //                contatoSalvo.Should().NotBeNull();
//    //                contatoSalvo.Email.Should().Be(contato.Email); // Verifique outros atributos se necessário

//    //                // Salvar o ID do contato criado para uso em outros testes, se necessário
//    //                createdContatoId = contatoSalvo.Id;
//    //            }

//    //            // Act - Deleção do Contato
//    //            var deleteResponse = await _client.DeleteAsync($"https://localhost:7196/api/contato/{contato.Email}");

//    //            // Assert - Verificação da Deleção
//    //            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

//    //            var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
//    //            deleteContent.Should().Be(ResponseMessages.ContatoApagado);

//    //            // Verifique se o contato foi removido corretamente do banco de dados
//    //            using (var scope = _serviceProvider.CreateScope())
//    //            {
//    //                var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
//    //                var contatoDeletado = await context.Contatos.FirstOrDefaultAsync(c => c.Id == createdContatoId);

//    //                contatoDeletado.Should().BeNull();
//    //            }


//    //        }
//    //        catch (Exception ex)
//    //        {

//    //            throw ex;
//    //        }


//    //    }
//    //}

//    #endregion

//    public class DeletarContatoTests : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;
//        private readonly IServiceProvider _serviceProvider;

//        public DeletarContatoTests(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateClient();
//            _serviceProvider = factory.Services;
//        }

//        public async Task DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Id_Correto()
//        {
//            try
//            {
//                // Arrange - Criação do Contato
//                var contato = new AutoFaker<RequisicaoRegistrarContatoJson>()
//                    .RuleFor(x => x.TelefoneProxy, 88888888)
//                    .RuleFor(x => x.PrefixoProxy, 99)
//                    .RuleFor(x => x.Email, new Faker().Person.Email)
//                    .Generate();

//                // Serializa o objeto contato para JSON
//                var body = new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json");

//                var createResponse = await _client.PostAsync("https://localhost:7196/api/contato", body);

//                createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
//                var jsonResponse = await createResponse.Content.ReadAsStringAsync();

//                jsonResponse.Should().Be(ResponseMessages.ContatoCriado);

//                // Extrair o ID do contato criado a partir da resposta
//                //var createdContatoId = JsonConvert.DeserializeObject<long>(jsonResponse);


//                // Extrair o ID do contato criado do banco de dados
//                long createdContatoId;
//                using (var scope = _serviceProvider.CreateScope())
//                {
//                    var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
//                    var contatoSalvo = await context.Contatos.FirstOrDefaultAsync(c => c.Email == contato.Email);

//                    contatoSalvo.Should().NotBeNull();
//                    contatoSalvo.Email.Should().Be(contato.Email); // Verifique outros atributos se necessário

//                    // Salvar o ID do contato criado para uso em outros testes, se necessário
//                    createdContatoId = contatoSalvo.Id;
//                }



//                // Act - Deleção do Contato
//                var deleteResponse = await _client.DeleteAsync($"https://localhost:7196/api/contato/{contato.Email}");

//                // Assert - Verificação da Deleção
//                deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

//                var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
//                deleteContent.Should().Be(ResponseMessages.ContatoApagado);

//                // Verifique se o contato foi removido corretamente do banco de dados
//                using (var scope = _serviceProvider.CreateScope())
//                {
//                    var context = scope.ServiceProvider.GetRequiredService<MinhaAgendaDeContatosContext>();
//                    var contatoDeletado = await context.Contatos.FirstOrDefaultAsync(c => c.Id == createdContatoId);

//                    contatoDeletado.Should().BeNull();
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//    }


//}
