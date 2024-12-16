using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IDivisionThreeService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(DivisionThreeDtoModel DivisionThree);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateDivisionThreeDtoModel DivisionThree);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> ImportDivisionThree(List<ImportDivisionThreeModel> DivisionThreelist);
    }
}