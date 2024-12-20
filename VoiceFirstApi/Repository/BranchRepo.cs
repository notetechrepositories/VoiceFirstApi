using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class BranchRepo : IBranchRepo
    {
        private readonly DapperContext _dapperContext;

        public BranchRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t2_company_branch(id_t2_company_branch,id_t1_company,t2_company_branch_name,t2_id_branch_type,
                t2_address_1,t2_address_2,t2_zip_code,t2_mobile_no,t2_phone_no,t2_email,id_t2_1_local,inserted_by,inserted_date) 
                VALUES (@Id,@CompanyId,@Name,@BranchType,@Address1,@Address2,@ZipCode,@Mobile,@PhoneNo,@Email,@Local,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "DELETE FROM t2_company_branch WHERE id_t2_company_branch = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<BranchModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "SELECT * FROM t2_company_branch ";

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
                  return await connection.QueryAsync<BranchModel>(query);
            }
        }

        public async Task<BranchModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t2_company_branch WHERE id_t2_company_branch = @id";

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
                return await connection.QuerySingleOrDefaultAsync<BranchModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t2_company_branch
                SET 
                    id_t1_company = @CompanyId, 
                    t2_company_branch_name = @Name, 
                    t2_id_branch_type = @BranchType, 
                    t2_address_1 = @Address1, 
                    t2_address_2 = @Address2, 
                    t2_zip_code = @ZipCode, 
                    t2_mobile_no = @Mobile, 
                    t2_phone_no = @PhoneNo, 
                    t2_email = @Email, 
                    id_t2_1_local = @Local, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t2_company_branch = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}