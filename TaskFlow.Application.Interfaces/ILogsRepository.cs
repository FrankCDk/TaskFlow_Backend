using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces
{
    public interface ILogsRepository
    {
        Task Create(Log log, CancellationToken cancellationToken);
    }
}
