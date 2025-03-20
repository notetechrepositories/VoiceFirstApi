using Dapper;
using VoiceFirstApi.Context;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.Repository
{
    public class SubSectionRepo : ISubSectionRepo
    {
        private readonly DapperContext _dapperContext;

        public SubSectionRepo(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<int> AddAsync(object parameters)
        {
            var query = @"
                INSERT INTO  t3_branch_sub_section(id_t3_branch_sub_section,sub_section_name,id_t3_branch_section,inserted_by,inserted_date) 
                VALUES (@Id,@SubSectionName,@BranchSectionId,@InsertedBy,@InsertedDate);";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var query = "UPDATE t3_branch_sub_section set is_delete=1,is_active=0 WHERE id_t3_branch_sub_section = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id });
            }
        }

        public async Task<IEnumerable<SubSectionModel>> GetAllAsync(Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t3_branch_sub_section ";

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
                return await connection.QueryAsync<SubSectionModel>(query);
            }
        }
        public async Task<SubSectionModel> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var query = "SELECT * FROM t3_branch_sub_section WHERE id_t3_branch_sub_section = @id";

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
                return await connection.QuerySingleOrDefaultAsync<SubSectionModel>(query, parameters);
            }
        }
        public async Task<int> UpdateStatus(string id, int status)
        {
            var query = "UPDATE t3_branch_sub_section set is_active=@status  WHERE id_t3_branch_sub_section = @id";
            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { id = id, status = status });
            }
        }
        public async Task<int> UpdateAsync(object parameters)
        {
            var query = @"
                UPDATE t3_branch_sub_section
                SET 
                    sub_section_name = @SubSectionName, 
                    id_t3_branch_section = @BranchSectionId, 
                    updated_by = @UpdatedBy, 
                    updated_date = @UpdatedDate
                WHERE id_t3_branch_sub_section = @Id";

            using (var connection = _dapperContext.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}
