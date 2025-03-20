using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface ISectionRepo
    {
        Task<IEnumerable<SectionModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<SectionModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> UpdateStatus(string id, int status);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}
