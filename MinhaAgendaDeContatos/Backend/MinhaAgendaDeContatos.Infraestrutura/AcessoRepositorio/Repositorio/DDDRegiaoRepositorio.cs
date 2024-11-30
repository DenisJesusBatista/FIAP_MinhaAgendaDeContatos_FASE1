using Microsoft.EntityFrameworkCore;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
public class DDDRegiaoRepositorio : IDDDRegiao
{
    private readonly MinhaAgendaDeContatosContext _contexto;

    public DDDRegiaoRepositorio(MinhaAgendaDeContatosContext contexto)
    {
        _contexto = contexto;

    }

    public async Task<IList<DDDRegiao>> RecuperarPorPrefixo(string prefixo)
    {
        return await _contexto.DDDRegiao.AsNoTracking()
        .Where(x => x.Prefixo == prefixo)
        .ToListAsync();

    }
}
