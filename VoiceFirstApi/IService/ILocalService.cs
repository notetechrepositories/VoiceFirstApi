using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ILocalService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(LocalDtoModel Local);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateLocalDtoModel Local);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
    }
}