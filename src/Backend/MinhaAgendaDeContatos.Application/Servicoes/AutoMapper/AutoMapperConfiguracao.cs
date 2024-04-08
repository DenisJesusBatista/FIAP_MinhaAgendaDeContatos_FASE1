using AutoMapper;
using MinhaAgendaDeContatos.Comunicacao.Resposta;
using MinhaAgendaDeContatos.Domain.Entidades;

namespace MinhaAgendaDeContatos.Application.Servicoes.AutoMapper;

/*Realizar a configuração do AutoMapper*/
public class AutoMapperConfiguracao: Profile
{
    public AutoMapperConfiguracao()
    {
        //Funcao    

        // Mapeamento de RequisicaoRegistrarContatoJson para Contato
        CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarContatoJson, Domain.Entidades.Contato>();
        
        CreateMap<Contato, ContatoJson>();

        CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarContatoJson, Domain.Entidades.Contato>()
          .ForMember(destino => destino.Prefixo, config => config.MapFrom(requisicao => requisicao.Prefixo));
    }
}
