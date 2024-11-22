using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IDivisionOneRepo
    {
        Task<IEnumerable<DivisionOneModel>> GetAllAsync(Dictionary<string, object> filters);
        Task<DivisionOneModel> GetByIdAsync(string id, Dictionary<string, object> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}