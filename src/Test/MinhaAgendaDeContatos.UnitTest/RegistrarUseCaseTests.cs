using AutoBogus;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging; // Importar o namespace do ILogger
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using Moq;

namespace MinhaAgendaDeContatos.UnitTest
{
    public class RegistrarUseCaseTests
    {
        private readonly Mock<IContatoReadOnlyRepositorio> _contatoReadOnlyRepositorio;
        private readonly Mock<IContatoWriteOnlyRepositorio> _contatoWriteOnlyRepositorio;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnidadeDeTrabalho> _unidadeDeTrabalho;
        private readonly Mock<ILogger<RegistrarContatoUseCase>> _logger; // Mock do ILogger
        private readonly IRegistrarContatoUseCase _useCase;

        public RegistrarUseCaseTests()
        {
            _contatoReadOnlyRepositorio = new Mock<IContatoReadOnlyRepositorio>();
            _contatoWriteOnlyRepositorio = new Mock<IContatoWriteOnlyRepositorio>();
            _mapper = new Mock<IMapper>();
            _unidadeDeTrabalho = new Mock<IUnidadeDeTrabalho>();
            _logger = new Mock<ILogger<RegistrarContatoUseCase>>(); // Inicializando o Mock do ILogger

            // Passando o mock do logger ao criar a instância do UseCase
            _useCase = new RegistrarContatoUseCase(
                _contatoWriteOnlyRepositorio.Object,
                _mapper.Object,
                _unidadeDeTrabalho.Object,
                _contatoReadOnlyRepositorio.Object,
                _logger.Object); // Passando o mock do logger
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Falso_Quando_Contato_Ja_Existente()
        {
            // Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Internet.Email())
                .RuleFor(x => x.Telefone, "99 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _useCase.Executar(requisicao);

            // Assert
            result.Should().BeFalse();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Falso_Quando_Email_Em_Branco()
        {
            // Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, "")
                .RuleFor(x => x.Telefone, "99 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _useCase.Executar(requisicao);

            // Assert
            result.Should().BeFalse();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Falso_Quando_Telefone_Em_Branco()
        {
            // Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Internet.Email())
                .RuleFor(x => x.Telefone, "")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _useCase.Executar(requisicao);

            // Assert
            result.Should().BeFalse();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Falso_Quando_Email_Fora_Do_Padrao()
        {
            // Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Random.String())
                .RuleFor(x => x.Telefone, "")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _useCase.Executar(requisicao);

            // Assert
            result.Should().BeFalse();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Falso_Quando_Telefone_Fora_Do_Padrao()
        {
            // Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Random.String())
                .RuleFor(x => x.Telefone, "999 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _useCase.Executar(requisicao);

            // Assert
            result.Should().BeFalse();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }
    }
}
