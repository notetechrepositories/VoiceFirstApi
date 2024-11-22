using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class TestRepo : ITestRepo
    {
        private readonly DapperContext _dapperContext;

        public TestRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO test(id,name,inserted_by,inserted_date) 
                VALUES (@Id,@Name,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM test WHERE id = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<TestModel>> GetAllAsync(Dictionary<string, object> filters)
        {
            var query = "SELECT * FROM test";

            if (filters != null && filters.Any())
            {
                var whereClauses = filters.Select(f => $"{f.Key} = @{f.Key}");
                query += " WHERE " + string.Join(" AND ", whereClauses);
            }

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<TestModel>(query, filters);
            }
        }

        public async Task<TestModel> GetByIdAsync(string id, Dictionary<string, object> filters)
        {
            var query = "SELECT * FROM test WHERE id = @id";

            if (filters != null && filters.Any())
            {
                var whereClauses = filters.Select(f => $"{f.Key} = @{f.Key}");
                query += " AND " + string.Join(" AND ", whereClauses);
            }

            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    parameters.Add(filter.Key, filter.Value);
                }
            }

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<TestModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE test
                SET 
                    name = @Name, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}