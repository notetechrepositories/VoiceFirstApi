using Dapper;
using VoiceFirstApi.Context;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.Repository
{
    public class ProgramRepo : IProgramRepo
    {
        private readonly DapperContext _dapperContext;
        public ProgramRepo(DapperContext dapperContext) 
        { 
            _dapperContext=dapperContext;
        }
        public async Task<IEnumerable<GetAllActionWithProgramId>> GetAllProgramLinkWithAction(Dictionary<string, string> filters)
        {
            var query = "select t6_link_program_with_program_action.id_t6_link_program_with_program_action," +
                " t6_link_program_with_program_action.id_t6_program,t6_link_program_with_program_action.id_t6_program_action," +
                "t6_program_action.t6_action from t6_link_program_with_program_action" +
                " inner join t6_program_action on t6_program_action.id_t6_program_action=t6_link_program_with_program_action.id_t6_program_action";
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
                return await connection.QueryAsync<GetAllActionWithProgramId>(query);
            }

        }


        public async Task<IEnumerable<ProgramModel>> GetAllPrograms(Dictionary<string, string> filters)
        {
            var query = "SELECT * from t6_program";
            using(var connection=_dapperContext.CreateConnection())
            {
                return await connection.QueryAsync<ProgramModel>(query);
            }
        }
    }
}
