using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
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
        public async Task<(Dictionary<string, object>, string, int)> ImportCountry(List<ImportCountryModel> import)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var Countrys = new List<CountryModel>();
            foreach ( var country in import)
            {
                var filter = new Dictionary<string, string>
                {
                        { "t2_1_country_name", country.country_name }
                };
                var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

                if (countryList == null)
                {
                    var generatedId = Guid.NewGuid().ToString();
                    var parameters = new
                    {
                        Id = generatedId.Trim(),
                        Name = country.country_name.Trim(),
                        Div1 = country.div1.Trim(),
                        Div2 = country.div2.Trim(),
                        Div3 = country.div3.Trim(),
                        InsertedBy = userId.Trim(),
                        InsertedDate = DateTime.UtcNow
                    };
                    var status=await _CountryRepo.AddAsync(parameters);
                    if (status > 0)
                    {
                        CountryModel obj = new CountryModel();
                        obj.id_t2_1_country = parameters.Id;
                        obj.t2_1_country_name = parameters.Name;
                        obj.t2_1_div1_called = parameters.Div1;
                        obj.t2_1_div2_called = parameters.Div2;
                        obj.t2_1_div3_called = parameters.Div3;
                        obj.inserted_by = parameters.InsertedBy;
                        obj.inserted_date = parameters.InsertedDate;

                        Countrys.Add(obj);
                    }
                }
            }
            if (Countrys.Count > 0)
            {
                data["Items"] = Countrys;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }
        public async Task<(Dictionary<string, object>, string, int)> AddAsync(CountryDtoModel CountryDtoModel)
        {
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t2_1_country_name", CountryDtoModel.t2_1_country_name }
            };

            var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (countryList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.FAILED_CODE);
            }

            var userId = GetCurrentUserId();

            var generatedId = Guid.NewGuid().ToString();



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
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateCountryDtoModel Country)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();

            var filter = new Dictionary<string, string>
                {
                    { "t2_1_country_name", Country.t2_1_country_name }
                };

            var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (countryList !=null && countryList.id_t2_1_country!= Country.id_t2_1_country)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.FAILED_CODE);
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
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CountryRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CountryRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _CountryRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        
    }
}