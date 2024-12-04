using MinhaAgendaDeContatos.Comunicacao.Resposta;


namespace MinhaAgendaDeContatos.Application.UseCases.DDDRegiao.RecuperarPorPrefixo;
public interface IRecuperarDDDRegiaoPorPrefixoUseCase
{
    Task<RespostaDDDRegiaoJson> Executar(string prefixo);
}
