using Microsoft.EntityFrameworkCore;
using MinhaAgendaDeContatos.Domain.Entidades;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
public class MinhaAgendaDeContatosContext : DbContext
{
    public MinhaAgendaDeContatosContext(DbContextOptions<MinhaAgendaDeContatosContext> options) : base(options) { }

    //Variavel
    public DbSet<Contato> Contatos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*Responsavel em fazer a configuraçoes necessaria para fazer a 
         * conexão da variavel com a tabela usuario*/
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MinhaAgendaDeContatosContext).Assembly);
    }
}
