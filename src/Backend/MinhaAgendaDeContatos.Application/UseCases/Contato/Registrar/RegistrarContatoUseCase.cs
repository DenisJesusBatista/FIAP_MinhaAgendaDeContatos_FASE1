using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar
{
    public class RegistrarContatoUseCase : IRegistrarContatoUseCase
    {
        private readonly IContatoReadOnlyRepositorio _contatoReadOnlyRepositorio;
        private readonly IContatoWriteOnlyRepositorio _contatoWriteOnlyRepositorio;
        private readonly IMapper _mapper;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        private readonly ILogger<RegistrarContatoUseCase> _logger;

        // Construtor com injeção de dependências
        public RegistrarContatoUseCase(
            IContatoWriteOnlyRepositorio contatoWriteOnlyRepositorio,
            IMapper mapper,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IContatoReadOnlyRepositorio contatoReadOnlyRepositorio
            ,
            ILogger<RegistrarContatoUseCase> logger)
        {
            _contatoWriteOnlyRepositorio = contatoWriteOnlyRepositorio;
            _mapper = mapper;
            _unidadeDeTrabalho = unidadeDeTrabalho;
            _contatoReadOnlyRepositorio = contatoReadOnlyRepositorio;
            _logger = logger;
        }

        public async Task<bool> Executar(RequisicaoRegistrarContatoJson requisicao)
        {
            try
            {
                // Valida dados de entrada
                await Validar(requisicao);

                // Mapeia a requisição para entidade
                var entidade = _mapper.Map<Domain.Entidades.Contato>(requisicao);
                entidade.DataCriacao = DateTime.UtcNow;

                // Adiciona a entidade no repositório
                await _contatoWriteOnlyRepositorio.Adicionar(entidade);

                // Commit da unidade de trabalho
                await _unidadeDeTrabalho.Commit();

                return true;
            }
            catch (Exception ex)
            {
                // Registra o erro no log
                _logger.LogError(ex, "Erro ao registrar o contato.");
                return false;
            }
        }

        private async Task Validar(RequisicaoRegistrarContatoJson requisicao)
        {
            // Validação de dados com FluentValidation
            var validator = new RegistrarContatoValidator();
            var resultado = validator.Validate(requisicao);

            // Verifica se o email já está registrado
            if (await EmailJaRegistrado(requisicao.Email))
            {
                resultado.Errors.Add(new ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_REGISTRADO));
            }

            // Se a validação falhar, lança exceção
            if (!resultado.IsValid)
            {
                var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrosDeValidacaoException(mensagensDeErro);
            }
        }

        // Método separado para verificar se o email já existe
        private async Task<bool> EmailJaRegistrado(string email)
        {
            return await _contatoReadOnlyRepositorio.ExisteUsuarioComEmail(email);
        }
    }
}
