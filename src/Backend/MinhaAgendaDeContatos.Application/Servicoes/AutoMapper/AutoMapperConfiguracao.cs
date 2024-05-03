using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;
using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;

/*Realizar a configuração do AutoMapper*/
[ExcludeFromCodeCoverage]
public class AutoMapperConfiguracao: Profile
{
    public AutoMapperConfiguracao()
    {
        //Funcao    

        // Mapeamento de RequisicaoRegistrarContatoJson para Contato
        CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarContatoJson, Domain.Entidades.Contato>();
        
        CreateMap<Contato, ContatoJson>();

        // Mapeamento de RequisicaoRegistrarDDDRegiaoJson para DDDRegiao
        //CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarContatoJson, Domain.Entidades.Contato>();

        CreateMap<DDDRegiao, DDDRegiaoJson>();


        CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarContatoJson, Domain.Entidades.Contato>()
          .ForMember(destino => destino.Prefixo, config => config.MapFrom(requisicao => requisicao.Prefixo));
    }
}
