using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface ISelectionValuesRepo
    {
        Task<IEnumerable<SelectionValuesModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<SelectionValuesModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
        Task<int> UpdateStatus(string id, int status);


        // ------------sys selection values---------------------------------------------------
        Task<IEnumerable<SysSelectionValuesModel>> GetAllSysAsync(Dictionary<string, string> filters);
        Task<SysSelectionValuesModel> GetSysByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddSysAsync(object parameters);
        Task<int> UpdateSysAsync(object parameters);
        Task<int> DeleteSysAsync(string id);
        Task<int> UpdateSysStatus(string id, int status);

        // ------------user selection values---------------------------------------------------


        Task<IEnumerable<UserSelectionValuesModel>> GetAllUserAsync(Dictionary<string, string> filters);
        Task<UserSelectionValuesModel> GetUserByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddUserAsync(object parameters);
        Task<int> UpdateUserAsync(object parameters);
        Task<int> DeleteUserAsync(string id);
        Task<int> UpdateUserStatus(string id, int status);
    }
}