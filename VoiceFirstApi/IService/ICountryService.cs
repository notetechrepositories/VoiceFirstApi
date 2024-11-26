using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ICountryService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(CountryDtoModel Country);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateCountryDtoModel Country);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> ImportCountry(List<ImportCountryModel> import);
    }
}