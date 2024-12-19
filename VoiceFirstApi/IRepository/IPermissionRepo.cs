using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IPermissionRepo
    {
        Task<IEnumerable<PermissionModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<PermissionModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
        Task<int> DeleteByRoleIdAsync(string id);
    }
}