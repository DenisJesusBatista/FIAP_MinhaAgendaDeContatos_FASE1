using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Produtor.RabbitMqProducer
{
    public interface IRabbitMqProducer
    {
        void PublishMessage<T>(string queueName, T message);
    }
}
