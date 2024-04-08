using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Exceptions;
using MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
using System.Net;

namespace MinhaAgendaDeContatos.Api.Filtros;

public class FiltroDasExceptions: IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is MinhaAgendaDeContatosExceptions)
        {
            TratarMinhaAgendaContatoException(context);
        }
        else
        {

        }
    }

    private void TratarMinhaAgendaContatoException(ExceptionContext context)
    {
        if(context.Exception is ErrosDeValidacaoException)
        {
            TratarErroDeValidacaoException(context);    
        }       
    }

    private void TratarErroDeValidacaoException(ExceptionContext context)
    {
        var erroDeValidacaoException = context.Exception as ErrosDeValidacaoException;  
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new RespostaErroJson(erroDeValidacaoException.MensagensDeErro));

    }

    private void LancarErroDesconhecido(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new RespostaErroJson(ResourceMensagensDeErro.ERRO_DESCONHECIDO));
    }
}
