using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface ICountryRepo
    {
        public Task<IEnumerable<CountryModel>> GetAllAsync(Dictionary<string, object> filters);


        public Task<CountryModel> GetByIdAsync(string id, Dictionary<string, object> filters);


        public Task<int> AddAsync(object parameters);


        public Task<int> UpdateAsync(object parameters);


        public Task<int> DeleteAsync(string id);
    }
}
