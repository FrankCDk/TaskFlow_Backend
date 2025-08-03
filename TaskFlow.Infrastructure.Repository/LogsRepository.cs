using Microsoft.Data.SqlClient;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repository.SqlServer;

namespace TaskFlow.Infrastructure.Repository
{
    public class LogsRepository : ILogsRepository
    {
        private readonly SqlDatabase _sqlDatabase;
        public LogsRepository(SqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public async Task Create(Log log, CancellationToken cancellationToken)
        {
            string query = "INSERT INTO Logs(LogLevel, Message, Exception, Source, EventDate, RequestPath, AdditionalData) " +
                "VALUES (@LogLevel, @Message, @Exception, @Source, @EventDate, @RequestPath, @AdditionalData);";

            var parameters = new Dictionary<string, object?>
            {
                ["@LogLevel"] = log.LogLevel,
                ["@Message"] = log.Message,
                ["@Exception"] = log.Exception,
                ["@Source"] = log.Source,
                ["@EventDate"] = log.EventDate,
                ["@RequestPath"] = log.RequestPath,
                ["@AdditionalData"] = log.AdditionalData
            };

            await _sqlDatabase.ExecuteNonQueryAsync(query, parameters, cancellationToken);
        }
    }
}
