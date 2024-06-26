﻿using AutoBogus;
using AutoMapper;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorId;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;
using Moq;

namespace MinhaAgendaDeContatos.UnitTest.UseCases
{
    public class RecuperarIdUseCaseTests
    {
        private readonly Mock<IContatoReadOnlyRepositorio> _repositorioReadOnly;
        private readonly Mock<IMapper> _mapper;
        private readonly IRecuperarPorIdUseCase _useCase;
        public RecuperarIdUseCaseTests()
        {
            _repositorioReadOnly = new Mock<IContatoReadOnlyRepositorio>();
            _mapper = new Mock<IMapper>();
            _useCase = new RecuperarPorIdUseCase(_repositorioReadOnly.Object, _mapper.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Com_Sucesso_Tipo_Correto()
        {
            //Arrange
            int id = new Faker().Random.Int();
            var repositorioResult = new AutoFaker<Contato>().Generate(new Faker().Random.Int(1, 1000));

            _repositorioReadOnly.Setup(x => x.RecuperarPorId(It.IsAny<int>())).ReturnsAsync(repositorioResult);

            //Act
            var result = await _useCase.Executar(id);

            //Assert
            result.Should().BeAssignableTo<RespostaContatoJson>();
            result.Contatos.Should().NotBeEmpty();
        }
    }
}
