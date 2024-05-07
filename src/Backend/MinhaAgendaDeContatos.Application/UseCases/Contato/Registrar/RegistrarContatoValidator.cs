using FluentValidation;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Exceptions;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public class RegistrarContatoValidator : AbstractValidator<RequisicaoRegistrarContatoJson>
{
    //Criando construtor com as regras


    public RegistrarContatoValidator()
    {
        //Validando que a propriedade não pode ser vazia
        RuleFor(c => c.Nome).NotEmpty().WithMessage(ResourceMensagensDeErro.NOME_CONTATO_EMBRANCO);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ResourceMensagensDeErro.EMAIL_CONTATO_EMBRANCO);
        RuleFor(c => c.Telefone).NotEmpty().WithMessage(ResourceMensagensDeErro.TELEFONE_CONTATO_EMBRANCO);

        // Definindo uma lista de prefixos válidos
        var prefixosValidos = new List<int> { 11,12,13,14,15,16,17,18,19,21,22,24,27,28,31,32,33,34,35,37,38,41,42,43,44,45,46,47
            ,48,49,51,53,54,55,61,62,63,64,65,66,67,68,69,71,73,74,75,77,79,81,82,83,84,85,86,87,88,89,91,92,93,94,95,96,97,98,99 };


        RuleFor(c => c.TelefoneProxy)
            .Must(x => x > 9999999 && x < 999999999)
            .WithMessage(ResourceMensagensDeErro.TELEFONE_CONTATO_EMBRANCO);

        RuleFor(c => c.PrefixoProxy)
            .Must(x => prefixosValidos.Contains((int)x));
            //.WithMessage(ResourceMensagensDeErro.PREFIXO_CONTATO_EMBRANCO);



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
