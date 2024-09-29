using AutoBogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using Moq;

namespace MinhaAgendaDeContatos.UnitTest
{
    public class UpdateUseCaseTests
    {
        private readonly Mock<IContatoReadOnlyRepositorio> _repositorioReadOnly;
        private readonly Mock<IContatoWriteOnlyRepositorio> _repositorioWriteOnly;
        private readonly Mock<IUnidadeDeTrabalho> _unidadeDeTrabalho;
        private readonly IUpdateContatoUseCase _useCase;

        public UpdateUseCaseTests()
        {
            _repositorioReadOnly = new Mock<IContatoReadOnlyRepositorio>();
            _repositorioWriteOnly = new Mock<IContatoWriteOnlyRepositorio>();
            _unidadeDeTrabalho = new Mock<IUnidadeDeTrabalho>();
            _useCase = new UpdateContatoUseCase(_repositorioReadOnly.Object, _repositorioWriteOnly.Object, _unidadeDeTrabalho.Object);
        }

        [Fact]
        public async Task Executar_Deve_Alterar_Com_Sucesso()
        {
            //Arrage
            var requisicao = new AutoFaker<RequisicaoAlterarContatoJson>().Generate();
            var repositorioResult = new AutoFaker<Contato>().Generate();
            _repositorioReadOnly.Setup(x => x.RecuperarPorEmail(It.IsAny<string>())).ReturnsAsync(repositorioResult);

            //Act
            await _useCase.Executar(requisicao);

            //Assert
            _repositorioReadOnly.Verify(x => x.RecuperarPorEmail(It.IsAny<string>()), Times.Once);
            _repositorioWriteOnly.Verify(x => x.Update(It.IsAny<Contato>()), Times.Once);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Falso_Quando_Contato_Nao_Existente()
        {
            //Arrage
            var requisicao = new AutoFaker<RequisicaoAlterarContatoJson>().Generate();
            _repositorioReadOnly.Setup(x => x.RecuperarPorEmail(It.IsAny<string>())).ReturnsAsync(null as Contato);

            //Act
            var result = await _useCase.Executar(requisicao);

            //Assert.
            result.Should().BeFalse();
            _repositorioReadOnly.Verify(x => x.RecuperarPorEmail(It.IsAny<string>()), Times.Once);
            _repositorioWriteOnly.Verify(x => x.Update(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }
    }
}
