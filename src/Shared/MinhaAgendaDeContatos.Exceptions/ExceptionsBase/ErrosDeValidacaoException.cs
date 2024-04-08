namespace MinhaAgendaDeContatos.Exceptions.ExceptionsBase;
public class ErrosDeValidacaoException: MinhaAgendaDeContatosExceptions
{
    public List<string> MensagensDeErro {  get; set; }

    //Construtor com a lista de erros
    public ErrosDeValidacaoException(List<string> mensagensDeErro)
    {
        MensagensDeErro = mensagensDeErro;
        
    }

}
