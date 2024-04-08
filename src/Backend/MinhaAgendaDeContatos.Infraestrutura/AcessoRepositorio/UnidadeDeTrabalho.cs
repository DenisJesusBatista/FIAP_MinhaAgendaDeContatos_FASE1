using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio;
public sealed class UnidadeDeTrabalho : IDisposable, IUnidadeDeTrabalho
{
    //Classe que libera a memoria
    private readonly MinhaAgendaDeContatosContext _contexto;
    private bool _disposed;

    public UnidadeDeTrabalho(MinhaAgendaDeContatosContext contexto)
    {
        _contexto = contexto;

    }

    public async Task Commit()
    {
        await _contexto.SaveChangesAsync();
    }


    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            //Faz a liberacao de memoria
            _contexto.Dispose();
        }
        _disposed = true;

    }
}
