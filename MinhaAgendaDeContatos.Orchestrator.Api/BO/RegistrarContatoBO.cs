

using Microsoft.Extensions.DependencyInjection;
using MinhaAgendaDeContatos.Application.UseCases.Contato.Registrar;
using MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient;
using MinhaAgendaDeContatos.Orchestrator.Api.Domain;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MinhaAgendaDeContatos.Orchestrator.Api.BO
{
    public class RegistrarContatoBO
    {       

        public RegistrarContatoBO()
        {
            
        }

        public void Process()
        {           

        }
    }
}
