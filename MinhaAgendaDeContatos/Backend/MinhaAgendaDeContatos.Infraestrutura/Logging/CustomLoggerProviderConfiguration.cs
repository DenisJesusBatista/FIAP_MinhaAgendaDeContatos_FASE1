using Microsoft.Extensions.Logging;

namespace MinhaAgendaDeContatos.Infraestrutura.Logging;
public class CustomLoggerProviderConfiguration
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public int EventId { get; set; } = 0;
}
