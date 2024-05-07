using AutoBogus;
using AutoMapper;
using Bogus;
using FluentAssertions;
using MinhaAgendaDeContatos.Application.UseCases.Contato.RecuperarPorPrefixo;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
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
    public class RecuperarUseCaseTests
    {
        private readonly Mock<IContatoReadOnlyRepositorio> _repositorioReadOnly;
        private readonly Mock<IMapper> _mapper;
        private readonly IRecuperarPorPrefixoUseCase _useCase;
        public RecuperarUseCaseTests()
        {
            _repositorioReadOnly = new Mock<IContatoReadOnlyRepositorio>();
            _mapper = new Mock<IMapper>();
            _useCase = new RecuperarPorPrefixoUseCase(_repositorioReadOnly.Object, _mapper.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Com_Sucesso_Tipo_Correto()
        {
            //Arrange
            var prefixo = new Faker().Random.String();
            var repositorioResult = new AutoFaker<Contato>().Generate(new Faker().Random.Int(1,1000));
            var repositorioDDDRegiaoResult = new AutoFaker<DDDRegiao>().Generate(new Faker().Random.Int(1, 1000));            
            _repositorioReadOnly.Setup(x => x.RecuperarPorPrefixo(It.IsAny<string>())).ReturnsAsync((repositorioResult, repositorioDDDRegiaoResult));
            


            //Act
            var result = await _useCase.Executar(prefixo);

            //Assert
            result.Should().BeAssignableTo<RespostaContatoJson>();
            result.Contatos.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Erro_Quando_Nenhum_Contato_Encontrado()
        {
            //Arrange
            var prefixo = new Faker().Random.String();
            var repositorioResult = new AutoFaker<Contato>().Generate(0);
            var repositorioDDDRegiaoResult = new AutoFaker<DDDRegiao>().Generate(new Faker().Random.Int(1, 1000));
            _repositorioReadOnly.Setup(x => x.RecuperarPorPrefixo(It.IsAny<string>())).ReturnsAsync((repositorioResult, repositorioDDDRegiaoResult));


            //Act
            var action = async() => await _useCase.Executar(prefixo);

            //Assert
            await action.Should().ThrowAsync<ErrosDeValidacaoException>();
        }
    }
}
