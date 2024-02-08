using LoggingApp.Shared.Entities;

namespace LoggingApp.Producer.Interfaces
{
    public interface ILogService
    {
        Task ProduceAsync(LogEntity logEntity);
    }
}
