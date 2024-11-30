using System;

namespace MinhaAgendaDeContatos.Produtor.RabbitMqSettings
{
    public class RabbitMqSettings
    {
        /// <summary>
        /// O nome do host RabbitMQ.
        /// </summary>
        public string HostName { get; set; } = string.Empty;

        /// <summary>
        /// A porta na qual o RabbitMQ está ouvindo.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// O nome de usuário para autenticação no RabbitMQ.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// A senha para autenticação no RabbitMQ.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// O virtual host RabbitMQ.
        /// </summary>
        public string VirtualHost { get; set; } = string.Empty;

        // Construtor padrão
        public RabbitMqSettings() { }

        // Construtor com parâmetros para facilitar a criação de instâncias
        public RabbitMqSettings(string hostName, int port, string userName, string password, string virtualHost)
        {
            HostName = hostName;
            Port = port;
            UserName = userName;
            Password = password;
            VirtualHost = virtualHost;
        }

        // Método para validação de configurações
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(HostName))
                throw new ArgumentException("O nome do host RabbitMQ não pode estar vazio.", nameof(HostName));

            if (Port <= 0)
                throw new ArgumentException("A porta do RabbitMQ deve ser maior que zero.", nameof(Port));

            if (string.IsNullOrWhiteSpace(UserName))
                throw new ArgumentException("O nome de usuário do RabbitMQ não pode estar vazio.", nameof(UserName));

            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("A senha do RabbitMQ não pode estar vazia.", nameof(Password));

            if (string.IsNullOrWhiteSpace(VirtualHost))
                throw new ArgumentException("O virtual host do RabbitMQ não pode estar vazio.", nameof(VirtualHost));
        }
    }
}
