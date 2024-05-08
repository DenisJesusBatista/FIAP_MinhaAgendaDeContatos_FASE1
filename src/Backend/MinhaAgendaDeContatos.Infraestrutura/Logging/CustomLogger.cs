using Microsoft.Extensions.Logging;

namespace MinhaAgendaDeContatos.Infraestrutura.Logging;
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
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        //Interpolacao de string $""
        string message = $"Log de Execução {logLevel}: {eventId} - {formatter(state, exception)}";

        if (Arquivo)
            EscreverTextoNoArquivo(message);
        else

            Console.WriteLine(message);
    }

    private void EscreverTextoNoArquivo(string message)
    {
        /*Escrevendo log no arquivo txt*/
        string caminhoArquivo = Environment.CurrentDirectory + @$"\LOG-{DateTime.Now:yyyy-MM-dd}.txt";

        if (!File.Exists(caminhoArquivo))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(caminhoArquivo));
            File.Create(caminhoArquivo).Dispose();
        }

        using StreamWriter stream = new(caminhoArquivo, true);
        stream.WriteLine(message);
        stream.Close();
    }
}
