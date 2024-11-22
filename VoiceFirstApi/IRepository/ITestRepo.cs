using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface ITestRepo
    {
        Task<IEnumerable<TestModel>> GetAllAsync(Dictionary<string, object> filters);
        Task<TestModel> GetByIdAsync(string id, Dictionary<string, object> filters);
        Task<int> AddAsync(object parameters);
        Task<int> UpdateAsync(object parameters);
        Task<int> DeleteAsync(string id);
    }
}