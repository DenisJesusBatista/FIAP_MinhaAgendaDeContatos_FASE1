﻿using MinhaAgendaDeContatos.Comunicacao.Requisicoes;
using MinhaAgendaDeContatos.Comunicacao.Resposta;

namespace MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
public interface IRegistrarContatoUseCase
{    
    Task<Domain.Entidades.Contato> Executar(RequisicaoRegistrarContatoJson requisicao);
}
