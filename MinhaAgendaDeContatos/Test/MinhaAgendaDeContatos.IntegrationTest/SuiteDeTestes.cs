
//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using FluentMigrator.Runner;
//using FluentMigrator.Runner.Processors;
//using global::MinhaAgendaDeContatos.Infraestrutura.Migrations;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//using Npgsql;
//using Testcontainers.PostgreSql;

//namespace MinhaAgendaDeContatos.IntegrationTest
//{
//    #region SuiteComentado

//    //public class SuiteDeTestes : IClassFixture<CustomWebApplicationFactory>
//    //{
//    //    private readonly CustomWebApplicationFactory _factory;

//    //    public SuiteDeTestes(CustomWebApplicationFactory factory)
//    //    {
//    //        _factory = factory;
//    //    }

//    //    private async Task ExecutarTestesEmSerie()
//    //    {
//    //        var registrarContatoTest = new RegistrarContatoTests(_factory);
//    //        await registrarContatoTest.RegistrarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Mensagem_Correta();

//    //        // Aguardar 10 segundos antes de chamar o segundo teste
//    //        //await Task.Delay(10000); // 10 segundos de atraso

//    //        var deletarContatoTest = new DeletarContatoTests(_factory);
//    //        //await Task.Delay(1000); // 10 segundos de atraso
//    //        await deletarContatoTest.DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Id_Correto();

//    //        var recuperarContatoIdTest = new RecuperarIdUseCaseTests(_factory);
//    //        //await Task.Delay(1000); // 10 segundos de atraso
//    //        await recuperarContatoIdTest.RecuperarContato_AposCriacao_DeveRetornar_Ok_ComContatoCorreto();
//    //    }

//    //    [Fact]
//    //    public async Task ExecutarSuiteDeTestesEmSerie()
//    //    {
//    //        // Limpar contêineres antes do teste
//    //        await _factory.LimparConteineres();
//    //        await _factory.IniciarConteineres();

//    //        // Executar os testes em série
//    //        await ExecutarTestesEmSerie();

//    //        // Limpar contêineres após o teste
//    //        await _factory.LimparConteineres();
//    //    }
//    //}
//    #endregion

//    public class SuiteDeTestes : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly CustomWebApplicationFactory _factory;

//        public SuiteDeTestes(CustomWebApplicationFactory factory)
//        {
//            _factory = factory;
//        }

//        private async Task ExecutarTestesEmSerie()
//        {
//            var registrarContatoTest = new RegistrarContatoTests(_factory);
//            await Task.Delay(1000); // Aguarde por 3 segundos antes de tentar novamente
//            await registrarContatoTest.RegistrarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Mensagem_Correta();
            
//            await Task.Delay(1000); // Aguarde por 3 segundos antes de tentar novamente
//            var deletarContatoTest = new DeletarContatoTests(_factory);            
//            await Task.Delay(1000); // Aguarde por 3 segundos antes de tentar novamente
//            await deletarContatoTest.DeletarContato_Quando_Sucesso_Deve_Retornar_Ok_Com_Id_Correto();
//            await Task.Delay(1000); // Aguarde por 3 segundos antes de tentar novamente

//            await Task.Delay(1000); // Aguarde por 3 segundos antes de tentar novamente
//            var recuperarContatoIdTest = new RecuperarIdUseCaseTests(_factory);
//            await Task.Delay(1000); // Aguarde por 3 segundos antes de tentar novamente
//            await recuperarContatoIdTest.RecuperarContato_AposCriacao_DeveRetornar_Ok_ComContatoCorreto();
//        }

//        [Fact]
//        public async Task ExecutarSuiteDeTestesEmSerie()
//        {
//            await _factory.LimparConteineres();
//            await _factory.IniciarConteineres();
//            await ExecutarTestesEmSerie();
//            await _factory.LimparConteineres();
//        }
//    }

//}

