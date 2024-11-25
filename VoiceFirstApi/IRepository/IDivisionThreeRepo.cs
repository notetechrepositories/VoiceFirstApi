using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IDivisionThreeRepo
    {
        Task<IEnumerable<DivisionThreeModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<DivisionThreeModel> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}