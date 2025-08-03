using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Repository.SqlServer;

namespace TaskFlow.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {

        private readonly SqlDatabase _sqlDatabase;

        public ProjectRepository(SqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public async Task<Project> CreateAsync(Project entity, CancellationToken cancellationToken)
        {
            string query = @"
                INSERT INTO Projects (Name, Description, CreatedAt) 
                OUTPUT INSERTED.Id 
                VALUES (@Name, @Description, @CreatedAt);";

            var parameters = new Dictionary<string, object>
            {
                ["@Name"] = entity.Name,
                ["@Description"] = entity.Description,
                ["@CreatedAt"] = DateTime.Now
            };

            var result = await _sqlDatabase.ExecuteInsertReturningAsync(query, parameters!, reader => new Project
            {
                Id = reader.GetInt32(0),
                Name = entity.Name,
                Description = entity.Description
            }, cancellationToken);

            return result;
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Project entity, CancellationToken cancellationToken)
        {
            string query = @"UPDATE Projects SET
                    Name = @Name,
                    Description = @Description,
                    UpdateAt = @UpdateAt
                    WHERE Id = @Id";

            var parameters = new Dictionary<string, object>
            {
                ["@Id"] = entity.Id,
                ["@Name"] = entity.Name,
                ["@Description"] = entity.Description,
                ["@UpdateAt"] = DateTime.Now
            };

            var result = await _sqlDatabase.ExecuteNonQueryAsync(query, parameters!, cancellationToken);

            return result > 0;
        }
    }
}
