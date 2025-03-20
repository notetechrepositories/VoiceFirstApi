using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface ISubSectionRepo
    {
        Task<IEnumerable<SubSectionModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<SubSectionModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> UpdateStatus(string id, int status);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}
