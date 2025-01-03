using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class DivisionThreeRepo : IDivisionThreeRepo
    {
        private readonly DapperContext _dapperContext;

        public DivisionThreeRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t2_1_div3(id_t2_1_div3,t2_1_div3_name,id_t2_1_div2,inserted_by,inserted_date) 
                VALUES (@Id,@Name,@Div2Id,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "delete FROM t2_1_div3  WHERE id_t2_1_div3 = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<DivisionThreeModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "SELECT t2_1_div3.id_t2_1_div3,t2_1_div3.id_t2_1_div2,t2_1_div3.t2_1_div3_name,t2_1_div2.t2_1_div2_name from t2_1_div3 " +
                "inner join t2_1_div2 on t2_1_div2.id_t2_1_div2 = t2_1_div3.id_t2_1_div2";

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
                      whereClauses = " t2_1_div3." + key + "='" + value + "'";
                  }
                  else
                  {
                      whereClauses += " AND t2_1_div3." + key + "='" + value + "'";
                  }
              }
              query += " WHERE " + whereClauses + ";";
          }

          using (var connection = _dapperContext.CreateConnection())
          {
                  return await connection.QueryAsync<DivisionThreeModel>(query);
          }
        }

        public async Task<DivisionThreeModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t2_1_div3 WHERE id_t2_1_div3 = @id";

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
                return await connection.QuerySingleOrDefaultAsync<DivisionThreeModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t2_1_div3
                SET 
                    t2_1_div3_name = @Name, 
                    id_t2_1_div2=@Div2Id,
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t2_1_div3 = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> UpdateStatus(string id, int status)
        {
            var query = "UPDATE t2_1_div3 set is_active=@status  WHERE id_t2_1_div3 = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id, status = status });
            }
        }
    }
}