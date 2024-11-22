using Dapper;
using VoiceFirstApi.Context;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.Repository
{
    public class ContryRepo :ICountryRepo
    {
        private readonly DapperContext _dapperContext;
        public ContryRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t2_1_country(id_t2_1_country,t2_1_country_name,t2_1_div1_called,
                t2_1_div2_called,t2_1_div3_called,inserted_by,inserted_date) 
                VALUES (@Id,@CountryName,@div1,@div2,@div3,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }

        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM t2_1_country WHERE id_t2_1_country = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<CountryModel>> GetAllAsync(Dictionary<string, object> filters)
        {
            var query = "SELECT * FROM t2_1_country";


            if (filters != null && filters.Any())
            {
                var whereClauses = filters.Select(f => $"{f.Key} = @{f.Key}");
                query += " WHERE " + string.Join(" AND ", whereClauses);
            }

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<CountryModel>(query, filters);
            }
        }

        public async Task<CountryModel> GetByIdAsync(string id, Dictionary<string, object> filters)
        {
            var query = "SELECT * FROM t2_1_country WHERE id_t2_1_country = @id";


            if (filters != null && filters.Any())
            {
                var whereClauses = filters.Select(f => $"{f.Key} = @{f.Key}");
                query += " AND " + string.Join(" AND ", whereClauses);
            }

            // Add the id to the parameters
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
                return await connection.QuerySingleOrDefaultAsync<CountryModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t2_1_country
                SET 
                    t2_1_country_name = @CountryName, 
                    t2_1_div1_called=@div1,
                    t2_1_div2_called=@div2,
                    t2_1_div3_called=@div3,
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t2_1_country = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                // Execute the query and return the number of affected rows (should be 1 if successful)
                return await connection.ExecuteAsync(query, parameters);
            }
            
        }
    }
}
