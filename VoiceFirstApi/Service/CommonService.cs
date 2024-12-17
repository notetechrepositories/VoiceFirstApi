using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VoiceFirstApi.Utilities;
using VoiceFirstApi.DtoModels;

namespace VoiceFirstApi.Service
{
    public class CommonService : ICommonService
    {
        private readonly IDivisionThreeRepo _DivisionThreeRepo;
        private readonly IDivisionTwoRepo _DivisionTwoRepo;
        private readonly IDivisionOneRepo _DivisionOneRepo;
        private readonly ICountryRepo _CountryRepo;

        public CommonService(IDivisionThreeRepo DivisionThreeRepo, IDivisionTwoRepo DivisionTwoRepo, IDivisionOneRepo DivisionOneRepo, ICountryRepo CountryRepo)
        {
            _DivisionThreeRepo = DivisionThreeRepo;
            _CountryRepo = CountryRepo;
            _DivisionTwoRepo = DivisionTwoRepo;
            _DivisionOneRepo = DivisionOneRepo;
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
        public async Task<(Dictionary<string, object>, string, int)> importDivisions(List<ImportDivisionThreeModel> DivisionThreelist)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var DivisionsError = new List<ImportDivisionThreeModel>();
            foreach (var division in DivisionThreelist)
            {
                if (division.t2_1_country_name != null)
                {
                    var filter = new Dictionary<string, string>
                    {
                        { "t2_1_country_name", division.t2_1_country_name }
                    };
                    var countryList = _CountryRepo.GetAllAsync(filter).Result.FirstOrDefault();

                    if (countryList == null)
                    {
                        DivisionsError.Add(division);
                    }
                    else
                    {
                        var erroObj = new ImportDivisionThreeModel();
                        erroObj.t2_1_country_name = division.t2_1_country_name;
                        var filterDivisionOne = new Dictionary<string, string>
                        {
                            { "id_t2_1_country", countryList.id_t2_1_country },
                            { "t2_1_div1_name", division.t2_1_div1_name }
                        };
                        var divisionOneList = _DivisionOneRepo.GetAllAsync(filterDivisionOne).Result.FirstOrDefault();
                        var divOneId = Guid.NewGuid().ToString();
                        if (divisionOneList == null)
                        {
                            
                            var parameters = new
                            {
                                Id = divOneId.Trim(),
                                Name = division.t2_1_div1_name.Trim(),
                                CountryId = countryList.id_t2_1_country.Trim(),
                                InsertedBy = userId.Trim(),
                                InsertedDate = DateTime.UtcNow
                            };
                            var status = await _DivisionOneRepo.AddAsync(parameters);
                            if (status == 0)
                            {
                                erroObj.t2_1_div1_name= parameters.Name;
                            }
                            
                        }
                        else
                        {
                            divOneId = divisionOneList.id_t2_1_div1;
                        }
                        var filterDivisionTwo = new Dictionary<string, string>
                        {
                             { "t2_1_div2_name", division.t2_1_div2_name },
                            { "id_t2_1_div1", divOneId }
                        };
                        var divisionTwoList = _DivisionTwoRepo.GetAllAsync(filterDivisionTwo).Result.FirstOrDefault();
                        var divTwoId = Guid.NewGuid().ToString();
                        if (divisionTwoList == null)
                        {
                            var parameters = new
                            {
                                Id = divTwoId.Trim(),
                                Div1Id = divOneId,
                                Name = division.t2_1_div2_name,
                                InsertedBy = userId.Trim(),
                                InsertedDate = DateTime.UtcNow
                            };

                            var status = await _DivisionTwoRepo.AddAsync(parameters);
                            if (status == 0)
                            {
                                erroObj.t2_1_div2_name = parameters.Name;
                            }
                        }
                        else
                        {
                            divTwoId = divisionTwoList.id_t2_1_div2;
                        }

                        var filterDivisionThree = new Dictionary<string, string>
                        {
                                { "id_t2_1_div2", divTwoId },
                                { "t2_1_div3_name", division.t2_1_div3_name }
                        };

                        var exsitList = _DivisionThreeRepo.GetAllAsync(filterDivisionThree).Result.FirstOrDefault();
                        var divThreeId = Guid.NewGuid().ToString();
                        if (exsitList == null)
                        {
                            var parameters = new
                            {
                                Id = divThreeId.Trim(),
                                Name = division.t2_1_div3_name.Trim(),
                                Div2Id = divTwoId.Trim(),
                                InsertedBy = userId.Trim(),
                                InsertedDate = DateTime.UtcNow
                            };

                            var status = await _DivisionThreeRepo.AddAsync(parameters);
                            if (status == 0)
                            {
                                erroObj.t2_1_div3_name = parameters.Name;
                            }
                        }

                        if(erroObj.t2_1_div1_name!=null || erroObj.t2_1_div2_name != null || erroObj.t2_1_div3_name != null)
                        {
                            DivisionsError.Add(erroObj);
                        }

                    }
                }
                else
                {
                    DivisionsError.Add(division);
                }
            }
            if (DivisionsError.Count > 0)
            {
                data["Items"] = DivisionsError;
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
            else
            {
                data["Items"] = DivisionThreelist;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
        }
    }
}
