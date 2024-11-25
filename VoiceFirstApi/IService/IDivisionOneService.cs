using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IDivisionOneService
    {
        Task<(Dictionary<string, object>, string)> AddAsync(DivisionOneDtoModel DivisionOne);
        Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateDivisionOneDtoModel DivisionOne);
        Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string)> DeleteAsync(string id);
    }
}