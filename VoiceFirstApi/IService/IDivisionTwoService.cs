using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IDivisionTwoService
    {
        Task<(Dictionary<string, object>, string, int)> AddAsync(DivisionTwoDtoModel DivisionTwo);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateDivisionTwoDtoModel DivisionTwo);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> ImportDivisionTwo(List<ImportDivisionTwoModel> DivisionTwolist);
    }
}