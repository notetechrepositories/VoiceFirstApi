using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IUserRepo
    {
        Task<IEnumerable<UserModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<UserModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> UpdatePasswordAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}