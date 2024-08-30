using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Produtor.RabbitMqProducer
{
    public interface IRabbitMqProducer
    {
        Task PublishMessageAsync<T>(string queueName, T message);
    }
}
