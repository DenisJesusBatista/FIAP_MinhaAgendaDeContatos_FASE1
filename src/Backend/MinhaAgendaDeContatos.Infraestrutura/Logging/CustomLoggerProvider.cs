using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace MinhaAgendaDeContatos.Infraestrutura.Logging;
[ExcludeFromCodeCoverage]
public class CustomLoggerProvider : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration _loggerConfig;
    private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig)
    {
        _loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggerConfig));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
