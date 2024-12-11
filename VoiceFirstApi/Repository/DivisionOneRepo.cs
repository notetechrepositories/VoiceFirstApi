using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class DivisionOneRepo : IDivisionOneRepo
    {
        private readonly DapperContext _dapperContext;

        public DivisionOneRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t2_1_div1 (id_t2_1_div1,id_t2_1_country,t2_1_div1_name,inserted_by,inserted_date) 
                VALUES (@Id,@CountryId,@Name,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM t2_1_div1  WHERE id_t2_1_div1 = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<DivisionOneModel>> GetAllAsync(Dictionary<string, string> filters)
        {
            var query = "SELECT t2_1_div1.id_t2_1_div1,t2_1_div1.id_t2_1_country,t2_1_div1.t2_1_div1_name,t2_1_country.t2_1_country_name FROM t2_1_div1 inner join t2_1_country on t2_1_country.id_t2_1_country=t2_1_div1.id_t2_1_country ";

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
                return await connection.QueryAsync<DivisionOneModel>(query);
            }
        }

        public async Task<DivisionOneModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT t2_1_div1.id_t2_1_div1,t2_1_div1.id_t2_1_country,t2_1_div1.t2_1_div1_name,t2_1_country.t2_1_country_name FROM t2_1_div1 inner join t2_1_country on t2_1_country.id_t2_1_country=t2_1_div1.id_t2_1_country WHERE id_t2_1_div1 = @id";

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
                query += whereClauses + ";";
            }

            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<DivisionOneModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t2_1_div1 
                SET 
                    t2_1_div1_name = @Name, 
                    id_t2_1_country = @CountryId, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t2_1_div1 = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}