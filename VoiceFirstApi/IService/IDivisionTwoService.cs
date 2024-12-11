using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IDivisionTwoService
    {
        Task<(Dictionary<string, object>, string)> AddAsync(DivisionTwoDtoModel DivisionTwo);
        Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateDivisionTwoDtoModel DivisionTwo);
        Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string)> ImportDivisionTwo(List<ImportDivisionTwoModel> DivisionTwolist);
    }
}