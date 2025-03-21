using Dapper;
using System.Diagnostics.Metrics;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Repository
{
    public class RoleRepo : IRoleRepo
    {
        private readonly DapperContext _dapperContext;

        public RoleRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO t5_1_m_user_roles(id_t5_1_m_user_roles,t5_1_m_user_roles_name,t5_1_m_all_location_access,
                t5_1_m_all_location_type,t5_1_m_only_assigned_location,id_t4_1_selection_values,t5_1_m_type_id,inserted_by,inserted_date) 
                VALUES (@Id,@Name,@AllLocationAccess,@AllLocationType,@OnlyAssignedLocation,@SelectionValue,@TypeId,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "UPDATE t5_1_m_user_roles set is_delete=1,is_active=0 WHERE id_t5_1_m_user_roles = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<RoleModel>> GetAllAsync(Dictionary<string, string> filters)
        {
          var query = "SELECT * FROM t5_1_m_user_roles ";

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
                  return await connection.QueryAsync<RoleModel>(query);
            }
        }

        public async Task<RoleModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t5_1_m_user_roles WHERE id_t5_1_m_user_roles = @id";

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
                return await connection.QuerySingleOrDefaultAsync<RoleModel>(query, parameters);
            }
        }

        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t5_1_m_user_roles
                SET 
                    t5_1_m_user_roles_name = @Name, 
                    t5_1_m_all_location_access=@AllLocationAccess,
                    t5_1_m_all_location_type=@AllLocationType,
                    t5_1_m_only_assigned_location=@OnlyAssignedLocation,
                    t5_1_m_type_id=@TypeId,
                    id_t4_1_selection_values=@SelectionValue,
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t5_1_m_user_roles = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> UpdateStatus(string id, int status)
        {
            var query = "UPDATE t5_1_m_user_roles set is_active=@status  WHERE id_t5_1_m_user_roles = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id, status = status });
            }
        }



        //------------------------------------ NeW Sys Role-------------------------------------
        public async Task<int> AddSysRoleAsync(object parameters)
        {
            var query = @"
                INSERT INTO t5_1_sys_roles(id_t5_1_sys_roles,t5_1_sys_roles_name,t5_1_sys_all_location_access,t5_1_sys_all_issues,inserted_by,inserted_date) 
                VALUES (@Id,@Name,@AllLocationAccess,@AllIssueAcces,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteSysRoleAsync(string id)
        {
            var query = "UPDATE t5_1_sys_roles set is_delete='y' WHERE id_t5_1_sys_roles = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<SysRoleModel>> GetAllSysRoleAsync(Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t5_1_sys_roles Where is_delete='n' ";

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

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<SysRoleModel>(query);
            }
        }
        public async Task<int> UpdateSysRoleAsync(object parameters)
        {
            var query = @"
                UPDATE t5_1_sys_roles
                SET 
                    t5_1_sys_roles_name = @Name, 
                    t5_1_sys_all_location_access=@AllLocationAccess,
                    t5_1_sys_all_issues=@AllIssueAcces,
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t5_1_sys_roles = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }


        //------------------------------------ NeW Company Role-------------------------------------
        public async Task<int> AddCompanyRoleAsync(object parameters)
        {
            var query = @"
                INSERT INTO t5_1_company_roles(id_t5_1_company_roles,t5_1_roles_name,id_t1_company,id_t5_1_sys_roles,t5_1_all_location_access,t5_1_all_issues,inserted_by,inserted_date) 
                VALUES (@Id,@Name,@CompanyId,@SysRoleId,@AllLocationAccess,@AllIssueAcces,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteCompanyRoleAsync(string id)
        {
            var query = "UPDATE t5_1_company_roles set is_delete='y' WHERE id_t5_1_company_roles = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<CompanyRoleModel>> GetAllCompanyRoleAsync(Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t5_1_company_roles Where is_delete='n' ";

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

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<CompanyRoleModel>(query);
            }
        }
        public async Task<int> UpdateCompanyRoleAsync(object parameters)
        {
            var query = @"
                UPDATE t5_1_company_roles
                SET 
                    t5_1_roles_name = @Name, 
                    id_t1_company = @CompanyId, 
                    id_t5_1_sys_roles = @SysRoleId, 
                    t5_1_all_location_access=@AllLocationAccess,
                    t5_1_all_issues=@AllIssueAcces,
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t5_1_company_roles = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}