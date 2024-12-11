using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IDivisionThreeService
    {
        Task<(Dictionary<string, object>, string)> AddAsync(DivisionThreeDtoModel DivisionThree);
        Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateDivisionThreeDtoModel DivisionThree);
        Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string)> ImportDivisionThree(List<ImportDivisionThreeModel> DivisionThreelist);
    }
}