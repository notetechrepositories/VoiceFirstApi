using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class LocalRepo : ILocalRepo
    {
        private readonly DapperContext _dapperContext;

        public LocalRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t2_1_local(id_t2_1_local,id_t2_1_country,id_t2_1_div1,id_t2_1_div2,id_t2_1_div3,t2_1_local_name,inserted_by,inserted_date) 
                VALUES (@Id,@CountryId,@Division1Id,@Division2Id,@Division3Id,@Name,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM t2_1_local WHERE id_t2_1_local = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<LocalModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "select t2_1_local.id_t2_1_local,t2_1_local.id_t2_1_country,t2_1_local.id_t2_1_div1,t2_1_local.id_t2_1_div2," +
                "t2_1_local.id_t2_1_div3,t2_1_local.t2_1_local_name,t2_1_country.t2_1_country_name," +
                "t2_1_div1.t2_1_div1_name,t2_1_div2.t2_1_div2_name,t2_1_div3.t2_1_div3_name from t2_1_local " +
                "inner join t2_1_country on t2_1_country.id_t2_1_country=t2_1_local.id_t2_1_country " +
                "inner join t2_1_div1 on t2_1_div1.id_t2_1_div1= t2_1_local.id_t2_1_div1 " +
                "inner join t2_1_div2 on t2_1_div2.id_t2_1_div2=t2_1_local.id_t2_1_div2 " +
                "inner join t2_1_div3 on t2_1_div3.id_t2_1_div3=t2_1_local.id_t2_1_div3 ";

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
                      whereClauses = " t2_1_local." + key + "='" + value + "'";
                  }
                  else
                  {
                      whereClauses += " AND t2_1_local." + key + "='" + value + "'";
                  }
              }
              query += " WHERE " + whereClauses + ";";
          }

          using (var connection = _dapperContext.CreateConnection())
          {
                  return await connection.QueryAsync<LocalModel>(query);
            }
        }

        public async Task<LocalModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "select t2_1_local.id_t2_1_local,t2_1_local.id_t2_1_country,t2_1_local.id_t2_1_div1,t2_1_local.id_t2_1_div2," +
                "t2_1_local.id_t2_1_div3,t2_1_local.t2_1_local_name,t2_1_country.t2_1_country_name," +
                "t2_1_div1.t2_1_div1_name,t2_1_div2.t2_1_div2_name,t2_1_div3.t2_1_div3_name from t2_1_local " +
                "inner join t2_1_country on t2_1_country.id_t2_1_country=t2_1_local.id_t2_1_country " +
                "inner join t2_1_div1 on t2_1_div1.id_t2_1_div1= t2_1_local.id_t2_1_div1 " +
                "inner join t2_1_div2 on t2_1_div2.id_t2_1_div2=t2_1_local.id_t2_1_div2 " +
                "inner join t2_1_div3 on t2_1_div3.id_t2_1_div3=t2_1_local.id_t2_1_div3  WHERE t2_1_local.id_t2_1_local = @id";

            if (filters != null && filters.Any())
            {
              var keys = new List<string>(filters.Keys);
              var whereClauses = "";
              for (int i = 0; i < keys.Count; i++)
              {
                  string key = keys[i];
                  string value = filters[key];
                  whereClauses += " AND t2_1_local." + key + "='" + value + "'";
              }
              query +=  whereClauses + ";";
            }

            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<LocalModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t2_1_local
                SET 
                    id_t2_1_country=@CountryId,
                    id_t2_1_div1=@Division1Id,
                    id_t2_1_div2=@Division2Id,
                    id_t2_1_div3=@Division3Id,
                    t2_1_local_name = @Name, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t2_1_local = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}