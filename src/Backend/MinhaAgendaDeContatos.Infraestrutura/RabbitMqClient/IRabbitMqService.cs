using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;


namespace MinhaAgendaDeContatos.Infraestrutura.RabbitMqClient
{
    public interface IRabbitMqService : IDisposable
    {
        RabbitMQ.Client.IModel CreateChannel();
    }
}
