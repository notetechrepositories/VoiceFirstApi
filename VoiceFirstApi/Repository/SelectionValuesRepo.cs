using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class SelectionValuesRepo : ISelectionValuesRepo
    {
        private readonly DapperContext _dapperContext;

        public SelectionValuesRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t4_1_selection_values(id_t4_1_selection_values,id_t4_selection,t4_1_selection_values_name,inserted_by,inserted_date) 
                VALUES (@Id,@SelectionId,@Name,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "UPDATE t4_1_selection_values  set is_delete=1,is_active=0 WHERE id_t4_1_selection_values = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<SelectionValuesModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "SELECT * FROM t4_1_selection_values ";

          if (filters != null && filters.Any())
          {
              var keys = new List<string>(filters.Keys);
              var whereClauses = "";
              for (int i = 0; i < keys.Count; i++)
              {
                  string key = keys[i];
                  string value = filters[key];
                  if (i == 0)
                  {
                      whereClauses = " " + key + "='" + value + "'";
                  }
                  else
                  {
                      whereClauses += " AND " + key + "='" + value + "'";
                  }
              }
              query += " WHERE " + whereClauses + ";";
          }

          using (var connection = _dapperContext.CreateConnection())
          {
                  return await connection.QueryAsync<SelectionValuesModel>(query);
            }
        }

        public async Task<SelectionValuesModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t4_1_selection_values WHERE id_t4_1_selection_values = @id";

            if (filters != null && filters.Any())
            {
              var keys = new List<string>(filters.Keys);
              var whereClauses = "";
              for (int i = 0; i < keys.Count; i++)
              {
                  string key = keys[i];
                  string value = filters[key];
                  whereClauses += " AND " + key + "='" + value + "'";
              }
              query +=  whereClauses + ";";
            }

            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SelectionValuesModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t4_1_selection_values
                SET 
                    id_t4_selection = @SelectionId, 
                    t4_1_selection_values_name = @Name, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t4_1_selection_values = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> UpdateStatus(string id, int status)
        {
            var query = "UPDATE t4_1_selection_values set is_active=@status  WHERE id_t4_1_selection_values = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id, status = status });
            }
        }
    }
}