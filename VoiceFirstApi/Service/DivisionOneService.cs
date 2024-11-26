using System.Diagnostics.Metrics;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class DivisionOneService : IDivisionOneService
    {
        private readonly IDivisionOneRepo _DivisionOneRepo;
        private readonly ICountryRepo _CountryRepo;

       
        public DivisionOneService(IDivisionOneRepo DivisionOneRepo, ICountryRepo countryRepo)
        {
            _DivisionOneRepo = DivisionOneRepo;
            _CountryRepo = countryRepo;
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

        public async Task<(Dictionary<string, object>, string)> AddAsync(DivisionOneDtoModel DivisionOneDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_country", DivisionOneDtoModel.id_t2_1_country },
                    { "t2_1_div1_name", DivisionOneDtoModel.t2_1_div1_name }
            };

            var exsitList = _DivisionOneRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST);
            }
            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = DivisionOneDtoModel.t2_1_div1_name.Trim(),
                CountryId = DivisionOneDtoModel.id_t2_1_country.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _DivisionOneRepo.AddAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateDivisionOneDtoModel DivisionOne)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();

            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_country", DivisionOne.id_t2_1_country },
                    { "t2_1_div1_name", DivisionOne.t2_1_div1_name }
            };

            var exsitList = _DivisionOneRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null && exsitList.id_t2_1_div1 != DivisionOne.id_t2_1_div1)
            {
                return (data, StatusUtilities.ALREADY_EXIST);
            }

            var parameters = new
            {
                Id = DivisionOne.id_t2_1_div1,
                Name = DivisionOne.t2_1_div1_name.Trim(),
                CountryId = DivisionOne.id_t2_1_country.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _DivisionOneRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionOneRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionOneRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionOneRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> ImportStateByCountry(List<ImportDivisionOneModel> importlist)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var Countrys = new List<DivisionOneModel>();
            foreach (var division in importlist)
            {
                if (division.Country_name != null)
                {
                    var filter = new Dictionary<string, string>
                    {
                        { "t2_1_country_name", division.Country_name }
                    };
                    var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

                    if (countryList == null)
                    {
                        return (data, StatusUtilities.FAILED);
                    }
                    else
                    {
                        division.Country_name = countryList.id_t2_1_country;
                    }

                }
                else
                {
                    return (data, StatusUtilities.FAILED);
                }

                
            }
            return (data, StatusUtilities.FAILED);
        }

        
    }
}