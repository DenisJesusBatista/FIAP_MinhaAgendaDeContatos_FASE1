using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Domain.Entidades;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
public class MinhaAgendaDeContatosContext : DbContext
{
    public MinhaAgendaDeContatosContext(DbContextOptions<MinhaAgendaDeContatosContext> options) : base(options) { }

    //Variavel
    public DbSet<Contato> Contatos { get; set; }

    //Variavel
    public DbSet<DDDRegiao> DDDRegiao { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*Responsavel em fazer a configuraçoes necessaria para fazer a 
         * conexão da variavel com a tabela usuario*/
        modelBuilder.Entity<Contato>()
            .HasOne(r => r.Regiao)
            .WithMany(c => c.Contatos)
            .HasForeignKey(c => c.Prefixo)
            .HasPrincipalKey(r => r.Prefixo);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_loggerFactory) // Habilita o log de SQL
            .EnableSensitiveDataLogging(); // Opcional: inclui parâmetros de consulta na saída do log            
    }

    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddFilter((category, level) =>
            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
            .AddConsole();
    });
}
