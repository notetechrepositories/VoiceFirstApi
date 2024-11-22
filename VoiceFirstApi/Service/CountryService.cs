using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepo _CountryRepo;

        public CountryService(ICountryRepo CountryRepo)
        {
            _CountryRepo = CountryRepo;
        }

        private string GetCurrentUserId()
        {
            return "abc1";
            /*var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in the token.");
            }
            return userIdClaim.Value;*/
        }

        public async Task<(Dictionary<string, object>, string)> AddAsync(CountryDtoModel CountryDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var filter = new Dictionary<string, object>
                {
                    { "t2_1_country_name", CountryDtoModel.t2_1_country_name }
                };

            var countryList = _CountryRepo.GetAllAsync(filter).Result;

            if(countryList.Count()>0)
            {
                return (data, StatusUtilities.ALREADY_EXIST);
            }
            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = CountryDtoModel.t2_1_country_name.Trim(),
                Div1 = CountryDtoModel.t2_1_div1_called.Trim(),
                Div2 = CountryDtoModel.t2_1_div2_called.Trim(),
                Div3 = CountryDtoModel.t2_1_div3_called.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _CountryRepo.AddAsync(parameters);

            if (status > 0)
            {
                data["data"] = parameters;
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateCountryDtoModel Country)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();

            var filter = new Dictionary<string, object>
                {
                    { "t2_1_country_name", Country.t2_1_country_name }
                };

            var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (countryList !=null && countryList.id_t2_1_country!= Country.id_t2_1_country)
            {
                return (data, StatusUtilities.ALREADY_EXIST);
            }

            var parameters = new
            {
                Id = Country.id_t2_1_country,
                Name = Country.t2_1_country_name.Trim(),
                Div1 = Country.t2_1_div1_called.Trim(),
                Div2 = Country.t2_1_div2_called.Trim(),
                Div3 = Country.t2_1_div3_called.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _CountryRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["data"] = parameters;
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, object> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CountryRepo.GetAllAsync(filters);
            data["data"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, object> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CountryRepo.GetByIdAsync(id, filters);
            data["data"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _CountryRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }
    }
}