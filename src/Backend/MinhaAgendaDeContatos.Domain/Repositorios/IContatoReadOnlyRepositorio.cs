using MinhaAgendaDeContatos.Domain.Entidades;

namespace MinhaAgendaDeContatos.Domain.Repositorios;
public interface IContatoReadOnlyRepositorio
{
    /*Write: Escrita*/
    /*Task: Adicição assicrona 
     Await: Para chamar
     */

    /*Verificar se existe usuario com email*/
    Task<bool> ExisteUsuarioComEmail(string email);
    Task<IEnumerable<Contato>> RecuperarPorPrefixo(string prefixo);
    Task<IEnumerable<Contato>> RecuperarPorId(int id);
    Task<IEnumerable<Contato>> RecuperarTodosContatos();
    Task<Entidades.Contato> RecuperarPorEmail(string email);
}
/*
 * ReadOnly: Ler a base de dados
 * WriteOnly: Escrever, insert e update
 * DeleteOnly: Deletar usario 
*/