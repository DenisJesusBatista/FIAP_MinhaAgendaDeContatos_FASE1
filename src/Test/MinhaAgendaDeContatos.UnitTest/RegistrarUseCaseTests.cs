﻿using AutoBogus;
using AutoMapper;
using Bogus;
using FluentAssertions;
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
        private readonly IRegistrarContatoUseCase _useCase;

        public RegistrarUseCaseTests()
        {
            _contatoReadOnlyRepositorio = new Mock<IContatoReadOnlyRepositorio>();
            _contatoWriteOnlyRepositorio = new Mock<IContatoWriteOnlyRepositorio>();
            _mapper = new Mock<IMapper>(); ;
            _unidadeDeTrabalho = new Mock<IUnidadeDeTrabalho>();
            _useCase = new RegistrarContatoUseCase(_contatoWriteOnlyRepositorio.Object, _mapper.Object, _unidadeDeTrabalho.Object, _contatoReadOnlyRepositorio.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Contato_Ja_Existente()
        {
            //Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Internet.Email())
                .RuleFor(x => x.Telefone, "99 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var action = async () => await _useCase.Executar(requisicao);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Nome_Em_Branco()
        {
            //Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Nome, "")
                .RuleFor(x => x.Email, new Faker().Internet.Email())
                .RuleFor(x => x.Telefone, "99 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var action = async () => await _useCase.Executar(requisicao);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Email_Em_Branco()
        {
            //Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, "")
                .RuleFor(x => x.Telefone, "99 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var action = async () => await _useCase.Executar(requisicao);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Telefone_Em_Branco()
        {
            //Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Internet.Email())
                .RuleFor(x => x.Telefone, "")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var action = async () => await _useCase.Executar(requisicao);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Email_Fora_Do_Padrao()
        {
            //Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Random.String())
                .RuleFor(x => x.Telefone, "")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var action = async () => await _useCase.Executar(requisicao);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Telefone_Fora_Do_Padrao()
        {
            //Arrange
            var requisicao = new AutoFaker<RequisicaoRegistrarContatoJson>()
                .RuleFor(x => x.Email, new Faker().Random.String())
                .RuleFor(x => x.Telefone, "999 9 9999-9999")
                .Generate();

            _contatoReadOnlyRepositorio.Setup(x => x.ExisteUsuarioComEmail(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var action = async () => await _useCase.Executar(requisicao);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
            _mapper.Verify(x => x.Map<Contato>(It.IsAny<RequisicaoRegistrarContatoJson>()), Times.Never);
            _contatoReadOnlyRepositorio.Verify(x => x.ExisteUsuarioComEmail(It.IsAny<string>()), Times.Once);
            _contatoWriteOnlyRepositorio.Verify(x => x.Adicionar(It.IsAny<Contato>()), Times.Never);
            _unidadeDeTrabalho.Verify(x => x.Commit(), Times.Never);
        }
    }
}
