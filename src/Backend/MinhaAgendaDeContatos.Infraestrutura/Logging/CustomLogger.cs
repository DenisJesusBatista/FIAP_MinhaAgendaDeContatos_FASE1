using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MinhaAgendaDeContatos.Infraestrutura.Logging
{
    public class CustomLogger : ILogger
    {
        private readonly string loggerName;
        private readonly CustomLoggerProviderConfiguration loggerConfig;
        public static bool Arquivo { get; set; } = false;

        public CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
        {
            this.loggerName = loggerName;
            this.loggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            // Implementar a lógica de escopo, se necessário.
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Verificar se o nível de log está habilitado com base na configuração
            return logLevel >= loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            // Formatar a mensagem de log
            string message = $"Log de Execução {logLevel}: {eventId} - {formatter(state, exception)}";

            // Escrever log no console ou no arquivo, com base na configuração
            if (Arquivo)
            {
                EscreverTextoNoArquivo(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        private void EscreverTextoNoArquivo(string message)
        {
            // Definir o caminho do arquivo de log
            string caminhoArquivo = Path.Combine(Environment.CurrentDirectory, $"LOG-{DateTime.Now:yyyy-MM-dd}.txt");

            try
            {
                // Criar diretório se não existir
                var diretorio = Path.GetDirectoryName(caminhoArquivo);
                if (diretorio != null && !Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                // Escrever mensagem no arquivo com controle de acesso para evitar exceções de arquivo em uso
                lock (caminhoArquivo)
                {
                    using StreamWriter stream = new(caminhoArquivo, append: true);
                    stream.WriteLine(message);
                }
            }
            catch (IOException ioEx)
            {
                // Tratar exceções relacionadas ao arquivo
                Console.WriteLine($"Erro ao escrever no arquivo de log: {ioEx.Message}");
            }
        }
    }
}
