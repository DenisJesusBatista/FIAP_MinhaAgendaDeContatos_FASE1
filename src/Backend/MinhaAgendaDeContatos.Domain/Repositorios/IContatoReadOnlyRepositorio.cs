using MinhaAgendaDeContatos.Comunicacao.Requisicoes;

namespace MinhaAgendaDeContatos.Domain.Repositorios;
public interface IContatoReadOnlyRepositorio
{
    /*Write: Escrita*/
    /*Task: Adicição assicrona 
     Await: Para chamar
     */

    /*Verificar se existe usuario com email*/
    Task<bool> ExisteUsuarioComEmail(RequisicaoRegistrarContatoJson requeisicao);

    Task<IList<Entidades.Contato>> RecuperarPorPrefixo(string prefixo);
    Task<IList<Entidades.Contato>> RecuperarPorId(int id);

    Task<IList<Entidades.Contato>> RecuperarTodosContatos();
    Task<Entidades.Contato> RecuperarPorEmail(string email);
}
/*
 * ReadOnly: Ler a base de dados
 * WriteOnly: Escrever, insert e update
 * DeleteOnly: Deletar usario 
*/