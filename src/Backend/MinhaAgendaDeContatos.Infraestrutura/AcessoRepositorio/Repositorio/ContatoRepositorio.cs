using Microsoft.EntityFrameworkCore;
using MinhaAgendaDeContatos.Domain.Entidades;
using MinhaAgendaDeContatos.Domain.Repositorios;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio.Repositorio;
public class ContatoRepositorio : IContatoWriteOnlyRepositorio, IContatoReadOnlyRepositorio, IContatoUpdateOnlyRepositorio
{
    private readonly MinhaAgendaDeContatosContext _contexto;

    public ContatoRepositorio(MinhaAgendaDeContatosContext contexto)
    {
        _contexto = contexto;

    }
    public async Task Adicionar(Contato contato)
    {
        await _contexto.Contatos.AddAsync(contato);
    }

    public async Task Deletar(string email)
    {
        var contato = await _contexto.Contatos.FirstOrDefaultAsync(c => c.Email == email);  
        _contexto.Contatos.Remove(contato);
    }

    public async Task<bool> ExisteUsuarioComEmail(string email)
    {
        return await _contexto.Contatos.AnyAsync(c => c.Email.Equals(email));
    }

    public async Task<Contato> RecuperarPorEmail(string email)
    {        
        return await _contexto.Contatos.FirstOrDefaultAsync(c => c.Email.Equals(email));
    }

    public async Task<(IList<Contato> Contatos, IList<DDDRegiao> Regioes)> RecuperarPorPrefixo(string prefixo)
    {
        var resultado = await (from contato in _contexto.Contatos.AsNoTracking()
                               join regiao in _contexto.DDDRegiao.AsNoTracking()
                               on contato.Prefixo equals regiao.prefixo
                               where contato.Prefixo == prefixo
                               select new { Contato = contato, Regiao = regiao })
                                .ToListAsync();

        return (resultado.Select(r => r.Contato).ToList(), resultado.Select(r => r.Regiao).ToList());
    }


    public async Task<(IList<Contato> Contatos, IList<DDDRegiao> Regioes)> RecuperarTodosContatos()
    {                  
        var resultado = await (from contato in _contexto.Contatos.AsNoTracking()
                           join regiao in _contexto.DDDRegiao.AsNoTracking()
                           on contato.Prefixo equals regiao.prefixo                           
                           select new { Contato = contato, Regiao = regiao })
                               .ToListAsync();

        return (resultado.Select(r => r.Contato).ToList(), resultado.Select(r => r.Regiao).ToList());
    }

    public async Task<(IList<Contato> Contatos, IList<DDDRegiao> Regioes)> RecuperarPorId(int id)
    {
        var resultado = await (from contato in _contexto.Contatos.AsNoTracking()
                               join regiao in _contexto.DDDRegiao.AsNoTracking()
                               on contato.Prefixo equals regiao.prefixo
                               where contato.Id == id
                               select new { Contato = contato, Regiao = regiao })
                                .ToListAsync();


        return (resultado.Select(r => r.Contato).ToList(), resultado.Select(r => r.Regiao).ToList());
      
    }


    public void Update(Contato contato)
    {
        _contexto.Contatos.Update(contato);
    }

    async Task IContatoWriteOnlyRepositorio.Update(Contato contato)
    {
        _contexto.Contatos.Update(contato);        
    }

}
