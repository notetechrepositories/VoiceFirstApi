using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class DivisionTwoRepo : IDivisionTwoRepo
    {
        private readonly DapperContext _dapperContext;

        public DivisionTwoRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t2_1_div2 (id_t2_1_div2,id_t2_1_div1,t2_1_div2_name,inserted_by,inserted_date) 
                VALUES (@Id,@Div1Id,@Name,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM t2_1_div2 WHERE id_t2_1_div2 = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<DivisionTwoModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "SELECT t2_1_div2.id_t2_1_div2,t2_1_div2.id_t2_1_div1,t2_1_div2.t2_1_div2_name,t2_1_div1.t2_1_div1_name from t2_1_div2 " +
                "inner join t2_1_div1 on t2_1_div1.id_t2_1_div1=t2_1_div2.id_t2_1_div1 ";

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
                      whereClauses = " t2_1_div2." + key + "='" + value + "'";
                  }
                  else
                  {
                      whereClauses += " AND t2_1_div2." + key + "='" + value + "'";
                  }
              }
              query += " WHERE " + whereClauses + ";";
          }

          using (var connection = _dapperContext.CreateConnection())
          {
                  return await connection.QueryAsync<DivisionTwoModel>(query);
            }
        }

        public async Task<DivisionTwoModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t2_1_div2 WHERE id_t2_1_div2 = @id";

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
                return await connection.QuerySingleOrDefaultAsync<DivisionTwoModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t2_1_div2
                SET 
                    t2_1_div2_name = @Name, 
                    id_t2_1_div1 = @Div1Id, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t2_1_div2 = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}