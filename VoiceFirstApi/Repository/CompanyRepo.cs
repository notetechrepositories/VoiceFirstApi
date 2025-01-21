using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class CompanyRepo : ICompanyRepo
    {
        private readonly DapperContext _dapperContext;

        public CompanyRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t1_company(id_t1_company,t1_company_name,id_company_type,id_currency,is_active_till_date,inserted_by,inserted_date) 
                VALUES (@Id,@Name,@Type,@Currency,@Date,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "UPDATE t1_company set is_delete=1,is_active=0  WHERE id_t1_company = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }
        public async Task<int> UpdateStatus(string id, int status)
        {
            var query = "UPDATE t1_company set is_active=@status  WHERE id_t1_company = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id, status = status });
            }
        }

        public async Task<IEnumerable<CompanyModel>> GetAllAsync(Dictionary<string, string> filters)
        {
            var query = "select t1_company.id_t1_company,t1_company.t1_company_name,t1_company.id_company_type,t4_1_selection_values.t4_1_selection_values_name as company_type,currency.t4_1_selection_values_name as curreny_name,t1_company.id_currency from t1_company" +
                  " LEFT JOIN t4_1_selection_values as currency on currency.id_t4_1_selection_values=t1_company.id_currency" +
                  " inner join t4_1_selection_values on t4_1_selection_values.id_t4_1_selection_values=t1_company.id_company_type ";

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
                      whereClauses = " t1_company." + key + "='" + value + "'";
                  }
                  else
                  {
                      whereClauses += " AND t1_company." + key + "='" + value + "'";
                  }
              }
                if (whereClauses != "")
                {
                    query += " WHERE " + whereClauses + ";";
                }
                
            
          }

          using (var connection = _dapperContext.CreateConnection())
          {
                  return await connection.QueryAsync<CompanyModel>(query);
            }
        }


        public async Task<CompanyModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t1_company WHERE id_t1_company = @id";

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
                return await connection.QuerySingleOrDefaultAsync<CompanyModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t1_company
                SET 
                    t1_company_name = @Name, 
                    id_company_type = @Type, 
                    id_currency = @Currency, 
                    is_active_till_date = @Date, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t1_company = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}