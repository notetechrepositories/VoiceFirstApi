using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IDivisionOneService
    {
        Task<(Dictionary<string, object>, string, int)> AddAsync(DivisionOneDtoModel DivisionOne);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateDivisionOneDtoModel DivisionOne);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);

        Task<(Dictionary<string, object>, string, int)> ImportStateByCountry(List<ImportDivisionOneModel> importlist);
    }
}