using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ICountryService
    {
        Task<(Dictionary<string, object>, string)> AddAsync(CountryDtoModel Country);
        Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateCountryDtoModel Country);
        Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, object> filters);
        Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, object> filters);
        Task<(Dictionary<string, object>, string)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string)> ImportCountry(List<ImportCountryModel> import);
    }
}