using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IRoleRepo
    {
        Task<IEnumerable<RoleModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<RoleModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
        Task<int> UpdateStatus(string id, int status);


        //------------------------------------ NeW Sys Role-------------------------------------


        Task<IEnumerable<SysRoleModel>> GetAllSysRoleAsync(Dictionary<string, string> filters);
        Task<int> AddSysRoleAsync(object parameters);
        Task<int> UpdateSysRoleAsync(object parameters);
        Task<int> DeleteSysRoleAsync(string id);


        //------------------------------------ NeW Company Role-------------------------------------


        Task<IEnumerable<CompanyRoleModel>> GetAllCompanyRoleAsync(Dictionary<string, string> filters);
        Task<int> AddCompanyRoleAsync(object parameters);
        Task<int> UpdateCompanyRoleAsync(object parameters);
        Task<int> DeleteCompanyRoleAsync(string id);
    }
}