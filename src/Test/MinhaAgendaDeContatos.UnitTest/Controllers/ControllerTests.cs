using AutoBogus;
using Bogus;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Api.Controllers;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Application.UseCases.DDDRegiao.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Produtor.RabbitMqProducer;
using Moq;
using System.Net;

namespace MinhaAgendaDeContatos.UnitTest.Controllers
{
    public class ControllerTests
    {
        private readonly Mock<IRegistrarContatoUseCase> _registrarUseCase;
        private readonly Mock<IRecuperarPorPrefixoUseCase> _recuperarUseCase;
        private readonly Mock<IRecuperarPorIdUseCase> _recuperarIdUseCase;
        private readonly Mock<IRecuperarTodosContatosUseCase> _recuperarTodosUseCase;
        private readonly Mock<IDeletarContatoUseCase> _deletarUseCase;
        private readonly Mock<IUpdateContatoUseCase> _updateUseCase;
        private readonly Mock<IRecuperarDDDRegiaoPorPrefixoUseCase> _recuperarRegiaoUseCase;
        private readonly ContatoController _controller;
        private readonly Mock<IRabbitMqProducer> _producer;

        public ControllerTests()
        {
            _registrarUseCase = new Mock<IRegistrarContatoUseCase>();
            _recuperarUseCase = new Mock<IRecuperarPorPrefixoUseCase>();
            _recuperarIdUseCase = new Mock<IRecuperarPorIdUseCase>();
            _recuperarTodosUseCase = new Mock<IRecuperarTodosContatosUseCase>();
            _deletarUseCase = new Mock<IDeletarContatoUseCase>();
            _updateUseCase = new Mock<IUpdateContatoUseCase>();
            _recuperarRegiaoUseCase = new Mock<IRecuperarDDDRegiaoPorPrefixoUseCase>();
            _producer = new Mock<IRabbitMqProducer>();
            _controller = new ContatoController(_producer.Object, new Mock<ILogger<ContatoController>>().Object);
        }

        [Fact]
        public async Task RegistrarContato_Deve_Chamar_UseCase_E_Retornar_Created()
        {
            //Arrange
            var request = new AutoFaker<RequisicaoRegistrarContatoJson>().Generate();

            //Act
            var result = await _controller.RegistrarContato(request);

            //Assert
            _producer.Verify(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<Object>()), Times.Once);
        }


        [Fact]
        public async Task RecuperarPorPrefixo_Deve_Chamar_UseCase_E_Retornar_Ok_Com_Tipo_Correto()
        {
            //Arrange
            var prefixo = new Faker().Random.Int().ToString();

            //Act
            var result = (OkObjectResult)await _controller.RecuperarPorPrefixo(prefixo);

            //Assert
            _producer.Verify(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<Object>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task RecuperarPorId_Deve_Chamar_UseCase_E_Retornar_Ok_Com_Tipo_Correto()
        {
            //Arrange
            int id = new Faker().Random.Int();

            //Act
            var result = (OkObjectResult)await _controller.RecuperarPorId(id);

            //Assert
            _producer.Verify(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<Object>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task RecuperarTodosContatos_Deve_Chamar_UseCase_E_Retornar_Ok_Com_Tipo_Correto()
        {
            //Arrange

            //Act
            var result = (OkObjectResult)await _controller.RecuperarTodosContatos();

            //Assert
            _producer.Verify(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<Object>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Deletar_Deve_Produzir_Evento_E_Retornar_NoContent()
        {
            //Arrange
            var email = new Faker().Random.String();

            //Act
            var result = (NoContentResult)await _controller.Deletar(email);

            //Assert
            _producer.Verify(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<Object>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateContato_Deve_Chamar_UseCase_E_Retornar_NoContent()
        {
            //Arrange
            var request = new AutoFaker<RequisicaoAlterarContatoJson>().Generate();

            //Act
            var result = (NoContentResult)await _controller.UpdateContato(request);

            //Assert
            _producer.Verify(x => x.PublishMessageAsync(It.IsAny<string>(), It.IsAny<Object>()), Times.Once);
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }
    }
}
