using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ISelectionService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(SelectionDtoModel Selection);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateSelectionDtoModel Selection);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
    }
}