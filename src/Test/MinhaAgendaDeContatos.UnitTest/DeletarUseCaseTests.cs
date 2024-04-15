using AutoBogus;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Deletar;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.UnitTest
{
    public class DeletarUseCaseTests
    {
        private readonly Mock<IContatoReadOnlyRepositorio> _repositorioReadOnly;
        private readonly Mock<IContatoWriteOnlyRepositorio> _repositorioWriteOnly;
        private readonly Mock<IUnidadeDeTrabalho> _unidadeDeTrabalho;
        private readonly IDeletarContatoUseCase _useCase;
        public DeletarUseCaseTests()
        {
            _repositorioReadOnly = new Mock<IContatoReadOnlyRepositorio>();
            _repositorioWriteOnly = new Mock<IContatoWriteOnlyRepositorio>();
            _unidadeDeTrabalho = new Mock<IUnidadeDeTrabalho>();
            _useCase = new DeletarContatoUseCase(_repositorioReadOnly.Object, _repositorioWriteOnly.Object, _unidadeDeTrabalho.Object);
        }

        [Fact]
        public async Task Executar_Deve_Deletar_Com_Sucesso()
        {
            //Arrange
            var email = new Faker().Random.String();
            var repositorioResult = new AutoFaker<Contato>().Generate();
            _repositorioReadOnly.Setup(x => x.RecuperarPorEmail(It.IsAny<string>())).ReturnsAsync(repositorioResult);

            //Act
            await _useCase.Executar(email);

            //Assert
            _repositorioReadOnly.Verify(x => x.RecuperarPorEmail(It.IsAny<string>()), Times.Once);
            _repositorioWriteOnly.Verify(x => x.Deletar(It.IsAny<string>()), Times.Once);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Contato_Nao_Existe()
        {
            //Arrange
            var email = new Faker().Random.String();
            _repositorioReadOnly.Setup(x => x.RecuperarPorEmail(It.IsAny<string>())).ReturnsAsync(null as Contato);

            //Act
            var action = async() => await _useCase.Executar(email);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _repositorioReadOnly.Verify(x => x.RecuperarPorEmail(It.IsAny<string>()), Times.Once);
            _repositorioWriteOnly.Verify(x => x.Deletar(It.IsAny<string>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }
    }
}
