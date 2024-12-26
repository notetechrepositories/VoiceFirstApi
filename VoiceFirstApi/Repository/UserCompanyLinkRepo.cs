using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class UserCompanyLinkRepo : IUserCompanyLinkRepo
    {
        private readonly DapperContext _dapperContext;

        public UserCompanyLinkRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t5_2_company_branch_users_link(id_t5_2_company_location_users_link,id_t5_users,t5_1_m_type_id,id_t4_1_selection_values,inserted_by,inserted_date) 
                VALUES (@Id,@UserId,@TypeId,@SelectionValueId,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM t5_2_company_branch_users_link WHERE id_t5_2_company_location_users_link = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<UserCompanyLinkModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "SELECT * FROM t5_2_company_branch_users_link ";

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
                  return await connection.QueryAsync<UserCompanyLinkModel>(query);
            }
        }

        public async Task<UserCompanyLinkModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t5_2_company_branch_users_link WHERE id_t5_2_company_location_users_link = @id";

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
                return await connection.QuerySingleOrDefaultAsync<UserCompanyLinkModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t5_2_company_branch_users_link
                SET 
                    id_t5_users = @Name, 
                    t5_1_m_type_id=@UserId,
                    id_t4_1_selection_values=@TypeId,
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t5_2_company_location_users_link = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}