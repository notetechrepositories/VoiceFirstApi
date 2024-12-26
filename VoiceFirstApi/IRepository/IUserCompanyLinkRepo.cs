using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IUserCompanyLinkRepo
    {
        Task<IEnumerable<UserCompanyLinkModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<UserCompanyLinkModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}