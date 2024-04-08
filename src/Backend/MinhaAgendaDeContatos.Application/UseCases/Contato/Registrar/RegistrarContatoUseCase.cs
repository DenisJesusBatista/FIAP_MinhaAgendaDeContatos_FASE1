using AutoMapper;
using MinhaAgendaDeContatos.Application.Servicoes.Token;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Repositorios;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public class RegistrarContatoUseCase: IRegistrarContatoUseCase
{
    //Variavel readonly só pode ser atribuido valor nela, apenas no construtor da classe ( public RegistrarContatoUseCase(IContatoWriteOnlyRepositorio repositorio) )
    private readonly IContatoReadOnlyRepositorio _contatoReadOnlyRepositorio;
    private readonly IContatoWriteOnlyRepositorio _contatoWriteOnlyRepositorio;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly TokenController _tokenController;

    //Configurar a injeção de dependência atalho CTOR - Criar 
    //Construtor
    public RegistrarContatoUseCase(IContatoWriteOnlyRepositorio contatoWriteOnlyRepositorio, IMapper mapper, IUnidadeDeTrabalho unidadeDeTrabalho,
        TokenController tokenController, IContatoReadOnlyRepositorio contatoReadOnlyRepositorio
        )
    {
        _contatoWriteOnlyRepositorio = contatoWriteOnlyRepositorio; 
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _tokenController = tokenController;
        _contatoReadOnlyRepositorio = contatoReadOnlyRepositorio;


    }

    public async Task<RespostaContatoRegistradoJson> Executar(RequisicaoRegistrarContatoJson requisicao)
    {
        await Validar(requisicao);

        //Conversão requisicao para entidade AutoMap
        //-Pluggin: AutoMapper na Application
        //-Pluggin: AutoMapper.Extensions.Microsoft.DependencyInjection na API para configurar para funcionar como injecao de dependencia

        var entidade = _mapper.Map<Domain.Entidades.Contato>(requisicao);

        //Salvar no banco de dados

        await _contatoWriteOnlyRepositorio.Adicionar(entidade);

        await _unidadeDeTrabalho.Commit();

        var token = _tokenController.GerarToken(entidade.Email);

        return new RespostaContatoRegistradoJson
        {
            Token = token,
        };

       
    }

    private async Task Validar(RequisicaoRegistrarContatoJson requisicao)
    {
        var validator = new RegistrarContatoValidator();
        var resultado = validator.Validate(requisicao);

        var existeContatoComEmail = await _contatoReadOnlyRepositorio.ExisteUsuarioComEmail(requisicao.Email);

        if (existeContatoComEmail)
        {
            resultado.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMensagensDeErro.EMAIL_JA_REGISTRADO));
        }

        if (!resultado.IsValid)
        {
            var mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }
    }
}
