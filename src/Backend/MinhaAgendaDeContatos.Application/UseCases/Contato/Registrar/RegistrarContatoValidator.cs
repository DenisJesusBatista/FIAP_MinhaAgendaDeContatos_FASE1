using FluentValidation;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Exceptions;
using System.Text.RegularExpressions;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public class RegistrarContatoValidator : AbstractValidator<RequisicaoRegistrarContatoJson>
{
    //Criando construtor com as regras


    public RegistrarContatoValidator()
    {
        //Validando que a propriedade não pode ser vazia
        RuleFor(c => c.Nome).NotEmpty().WithMessage(ResourceMensagensDeErro.NOME_CONTATO_EMBRANCO);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ResourceMensagensDeErro.EMAIL_CONTATO_EMBRANCO);


        RuleFor(c => c.TelefoneProxy)
            .Must(x => x > 9999999 && x < 999999999)
            .WithMessage(ResourceMensagensDeErro.TELEFONE_CONTATO_EMBRANCO);

        RuleFor(c => c.PrefixoProxy)
            .Must(x => x <= 99 && x >= 10)
            .WithMessage(ResourceMensagensDeErro.PREFIXO_CONTATO_EMBRANCO);



        //Validação do formato do e-mail
        //Quando uma regra for atendida, execute a função
        When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ResourceMensagensDeErro.EMAIL_CONTATO_INVALIDO);
        });

        //telefone numérico 
        //Validando telefone por regex
        //When(c => !string.IsNullOrWhiteSpace(c.Telefone), () =>
        //{
        //    RuleFor(c => c.Telefone).Custom((telefone, contexto) =>
        //    {
        //        string padraoTelefone = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
        //        var isValid = Regex.IsMatch(telefone, padraoTelefone);
        //        if(!isValid) 
        //        {
        //            contexto.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(telefone), ResourceMensagensDeErro.TELEFONE_CONTATO_INVALIDO));
        //        }
        //    }); 
        //});
    }
}
