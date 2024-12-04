namespace MinhaAgendaDeContatos.Domain.Repositorios;
public interface IContatoWriteOnlyRepositorio
{
    /*Write: Escrita*/
    /*Task: Adicição assicrona 
     Await: Para chamar
     */
    Task Adicionar(Entidades.Contato contato);
    Task Deletar(string email);

    Task Update(Entidades.Contato contato);
}

/*
 * ReadOnly: Ler a base de dados
 * WriteOnly: Escrever, insert e update
 * DeleteOnly: Deletar usario 
*/
