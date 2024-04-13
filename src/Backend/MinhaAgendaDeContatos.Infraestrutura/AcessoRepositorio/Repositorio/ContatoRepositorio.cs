using Microsoft.EntityFrameworkCore;
using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
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
    public async Task Adicionar(Contato contato) => await _contexto.Contatos.AddAsync(contato);


    public async Task Deletar(string email)
    {
        var contato = await _contexto.Contatos.FirstOrDefaultAsync(c => c.Email == email);
        _contexto.Contatos.Remove(contato);
    }

    public async Task<bool> ExisteUsuarioComEmail(RequisicaoRegistrarContatoJson requisicao)
    {
        return await _contexto.Contatos.AnyAsync(c => c.Email.Equals(requisicao.Email));
    }

    public async Task<Contato> RecuperarPorEmail(string email)
    {
        return await _contexto.Contatos.FirstOrDefaultAsync(c => c.Email.Equals(email));
    }

    public async Task<IList<Contato>> RecuperarPorPrefixo(string prefixo)
    {
        return await _contexto.Contatos.AsNoTracking()
        .Where(x => x.Prefixo == prefixo)
        .ToListAsync();
    }

    public async Task<IList<Contato>> RecuperarTodosContatos()
    {
        return await _contexto.Contatos.AsNoTracking().ToListAsync();
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
