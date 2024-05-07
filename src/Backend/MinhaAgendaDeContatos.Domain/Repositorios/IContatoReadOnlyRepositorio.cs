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
    Task<(IList<Contato> Contatos, IList<DDDRegiao> Regioes)> RecuperarPorPrefixo(string prefixo);
    Task<(IList<Contato> Contatos, IList<DDDRegiao> Regioes)> RecuperarPorId(int id);
    Task<(IList<Contato> Contatos, IList<DDDRegiao> Regioes)> RecuperarTodosContatos();
    Task<Entidades.Contato> RecuperarPorEmail(string email);
}
/*
 * ReadOnly: Ler a base de dados
 * WriteOnly: Escrever, insert e update
 * DeleteOnly: Deletar usario 
*/