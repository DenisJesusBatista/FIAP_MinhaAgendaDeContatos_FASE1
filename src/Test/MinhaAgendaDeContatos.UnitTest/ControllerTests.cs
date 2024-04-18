using AutoBogus;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using MinhaAgendaDeContatos.Api.Controllers;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.UnitTest
{
    public class ControllerTests
    {
        private readonly Mock<IRegistrarContatoUseCase> _registrarUseCase;
        private readonly Mock<IRecuperarPorPrefixoUseCase> _recuperarUseCase;
        private readonly Mock<IRecuperarPorIdUseCase> _recuperarIdUseCase;
        private readonly Mock<IRecuperarTodosContatosUseCase> _recuperarTodosUseCase;
        private readonly Mock<IDeletarContatoUseCase> _deletarUseCase;
        private readonly Mock<IUpdateContatoUseCase> _updateUseCase;
        private readonly ContatoController _controller;

        public ControllerTests()
        {
            _registrarUseCase = new Mock<IRegistrarContatoUseCase>();
            _recuperarUseCase = new Mock<IRecuperarPorPrefixoUseCase>();
            _recuperarIdUseCase = new Mock<IRecuperarPorIdUseCase>();
            _recuperarTodosUseCase = new Mock<IRecuperarTodosContatosUseCase>();
            _deletarUseCase = new Mock<IDeletarContatoUseCase>();
            _updateUseCase = new Mock<IUpdateContatoUseCase>();
            _controller = new ContatoController();
        }

        [Fact]
        public async Task RegistrarContato_Deve_Chamar_UseCase_E_Retornar_Created()
        {
            //Arrange
            var request = new AutoFaker<RequisicaoRegistrarContatoJson>().Generate();

            //Act
            var result = await _controller.RegistrarContato(_registrarUseCase.Object, request);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            createdResult.ActionName.Should().Be(nameof(Application.UseCases.Contato.RecuperarPorId));
            createdResult.RouteValues["id"].Should().Be(request.Id);

            _registrarUseCase.Verify(x => x.Executar(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Once);
        }


        [Fact]
        public async Task RecuperarPorPrefixo_Deve_Chamar_UseCase_E_Retornar_Ok_Com_Tipo_Correto()
        {
            //Arrange
            var prefixo = new Faker().Random.String();

            var useCaseResult = new AutoFaker<RespostaContatoJson>().Generate();
            _recuperarUseCase.Setup(x => x.Executar(It.IsAny<string>())).ReturnsAsync(useCaseResult);

            //Act
            var result = (OkObjectResult)await _controller.RecuperarPorPrefixo(_recuperarUseCase.Object, prefixo);

            //Assert
            _recuperarUseCase.Verify(x => x.Executar(It.IsAny<string>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<RespostaContatoJson>();
        }

        [Fact]
        public async Task RecuperarPorId_Deve_Chamar_UseCase_E_Retornar_Ok_Com_Tipo_Correto()
        {
            //Arrange
            int id = new Faker().Random.Int();

            var useCaseResult = new AutoFaker<RespostaContatoJson>().Generate();
            _recuperarIdUseCase.Setup(x => x.Executar(It.IsAny<int>())).ReturnsAsync(useCaseResult);

            //Act
            var result = (OkObjectResult)await _controller.RecuperarPorId(_recuperarIdUseCase.Object, id);
            
   
            //Assert
            _recuperarIdUseCase.Verify(x => x.Executar(It.IsAny<int>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<RespostaContatoJson>();
        }

        [Fact]
        public async Task RecuperarTodosContatos_Deve_Chamar_UseCase_E_Retornar_Ok_Com_Tipo_Correto()
        {
            //Arrange
            var useCaseResult = new AutoFaker<RespostaContatoJson>().Generate();
            _recuperarTodosUseCase.Setup(x => x.Executar()).ReturnsAsync(useCaseResult);

            //Act
            var result = (OkObjectResult)await _controller.RecuperarTodosContatos(_recuperarTodosUseCase.Object);

            //Assert
            _recuperarTodosUseCase.Verify(x => x.Executar(), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeOfType<RespostaContatoJson>();
        }

        [Fact]
        public async Task Deletar_Deve_Chamar_UseCase_E_Retornar_NoContent()
        {
            //Arrange
            var email = new Faker().Random.String();

            //Act
            var result = (NoContentResult)await _controller.Deletar(_deletarUseCase.Object, email);

            //Assert
            _deletarUseCase.Verify(x => x.Executar(It.IsAny<string>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateContato_Deve_Chamar_UseCase_E_Retornar_NoContent()
        {
            //Arrange
            var request = new AutoFaker<RequisicaoAlterarContatoJson>().Generate();

            //Act
            var result = (NoContentResult)await _controller.UpdateContato(_updateUseCase.Object, request);

            //Assert
            _updateUseCase.Verify(x => x.Executar(It.IsAny<RequisicaoAlterarContatoJson>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}
