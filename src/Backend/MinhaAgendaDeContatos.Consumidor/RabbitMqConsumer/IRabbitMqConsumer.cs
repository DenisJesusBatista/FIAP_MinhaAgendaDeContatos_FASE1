using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Consumidor.RabbitMqConsumer
{
    public interface IRabbitMqConsumer
    {
        public void ConsumeMessage();
    }
}
