using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ITestService
    {
        Task<(Dictionary<string, object>, string)> AddAsync(TestDtoModel Test);
        Task<(Dictionary<string, object>, string)> UpdateAsync(TestDtoModel Test);
        Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, object> filters);
        Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, object> filters);
        Task<(Dictionary<string, object>, string)> DeleteAsync(string id);
    }
}