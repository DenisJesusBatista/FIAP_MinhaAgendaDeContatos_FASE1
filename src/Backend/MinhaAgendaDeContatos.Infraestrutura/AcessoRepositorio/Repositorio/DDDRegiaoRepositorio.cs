using Microsoft.EntityFrameworkCore;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;
using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
[ExcludeFromCodeCoverage]
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
