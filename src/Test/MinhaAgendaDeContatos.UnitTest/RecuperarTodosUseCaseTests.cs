using AutoBogus;
using AutoMapper;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarTodos;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.UnitTest
{
    public class RecuperarTodosUseCaseTests
    {
        private readonly Mock<IContatoReadOnlyRepositorio> _repositorioReadOnly;
        private readonly Mock<IMapper> _mapper;
        private readonly IRecuperarTodosContatosUseCase _useCase;
        public RecuperarTodosUseCaseTests()
        {
            _repositorioReadOnly = new Mock<IContatoReadOnlyRepositorio>();
            _mapper = new Mock<IMapper>();
            _useCase = new RecuperarTodosContatosUseCase(_repositorioReadOnly.Object, _mapper.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Com_Sucesso_Tipo_Correto()
        {
            //Arrange
            var repositorioResult = new AutoFaker<Contato>().Generate(new Faker().Random.Int(0, 1000));
            var repositorioDDDRegiaoResult = new AutoFaker<DDDRegiao>().Generate(new Faker().Random.Int(0, 1000));
            
            _repositorioReadOnly.Setup(x => x.RecuperarTodosContatos()).ReturnsAsync((repositorioResult, repositorioDDDRegiaoResult));

            //Act
            var result = await _useCase.Executar();

            //Assert
            result.Should().BeAssignableTo<RespostaContatoJson>();
        }
    }
}
