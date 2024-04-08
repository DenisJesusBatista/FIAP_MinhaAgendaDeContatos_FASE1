using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Update;
public interface IUpdateContatoUseCase
{
    Task Executar(RequisicaoAlterarContatoJson requisicao);

    Task<Domain.Entidades.Contato> RecuperarPorEmail(string email);
}
