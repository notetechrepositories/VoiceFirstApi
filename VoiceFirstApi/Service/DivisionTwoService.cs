using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class DivisionTwoService : IDivisionTwoService
    {
        private readonly IDivisionTwoRepo _DivisionTwoRepo;
        private readonly IDivisionOneRepo _DivisionOneRepo;

        public DivisionTwoService(IDivisionTwoRepo DivisionTwoRepo,IDivisionOneRepo DivisionOneRepo)
        {
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

        public async Task<(Dictionary<string, object>, string)> AddAsync(DivisionTwoDtoModel DivisionTwoDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_div1", DivisionTwoDtoModel.id_t2_1_div1 },
                    { "t2_1_div2_name", DivisionTwoDtoModel.t2_1_div2_name }
            };

            var exsitList = _DivisionTwoRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST);
            }
            var parameters = new
            {
                Id = generatedId.Trim(),
                Div1Id = DivisionTwoDtoModel.id_t2_1_div1,
                Name = DivisionTwoDtoModel.t2_1_div2_name,
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _DivisionTwoRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateDivisionTwoDtoModel DivisionTwo)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_div1", DivisionTwo.id_t2_1_div1 },
                    { "t2_1_div2_name", DivisionTwo.t2_1_div2_name }
            };

            var exsitList = _DivisionTwoRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null && exsitList.id_t2_1_div2 != DivisionTwo.id_t2_1_div2)
            {
                return (data, StatusUtilities.ALREADY_EXIST);
            }

            var parameters = new
            {
                Id = DivisionTwo.id_t2_1_div2,
                Div1Id = DivisionTwo.id_t2_1_div1,
                Name = DivisionTwo.t2_1_div2_name,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _DivisionTwoRepo.UpdateAsync(parameters);

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
            var list = await _DivisionTwoRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionTwoRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionTwoRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> ImportDivisionTwo(List<ImportDivisionTwoModel> DivisionTwolist)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var Divisions = new List<DivisionTwoModel>();
            foreach (var division in DivisionTwolist)
            {
                if (division.Division_one_name != null && division.country_name != null)  
                {
                    var filter = new Dictionary<string, string>
                    {
                        { "t2_1_div1_name", division.Division_one_name },
                        { "id_t2_1_country", division.country_name }
                    };
                    var DivisionList = _DivisionOneRepo.GetAllAsync(filter).Result.FirstOrDefault();

                    if (DivisionList == null)
                    {
                        return (data, StatusUtilities.DIVISION_ONE_NOT_EXSISTS);
                    }
                    else
                    {
                        division.Division_one_name = DivisionList.id_t2_1_div1;
                    }

                }
                else
                {
                    return (data, StatusUtilities.DIVISION_ONE_NOT_EXSISTS);
                }


            }
            foreach (var division in DivisionTwolist)
            {
                var generatedId = Guid.NewGuid().ToString();
                var filter = new Dictionary<string, string>
                {
                    { "id_t2_1_div1", division.Division_one_name},
                    { "t2_1_div2_name", division.Division_two_name }
                };

                var exsitList = _DivisionTwoRepo.GetAllAsync(filter).Result.FirstOrDefault();

                if (exsitList == null)
                {
                    var parameters = new
                    {
                        Id = generatedId.Trim(),
                        Name = division.Division_two_name.Trim(),
                        Div1Id = division.Division_one_name.Trim(),
                        InsertedBy = userId.Trim(),
                        InsertedDate = DateTime.UtcNow
                    };

                    var status = await _DivisionTwoRepo.AddAsync(parameters);

                    if (status > 0)
                    {
                        DivisionTwoModel obj = new DivisionTwoModel();
                        obj.id_t2_1_div1 = parameters.Div1Id;
                        obj.id_t2_1_div2 = parameters.Id;
                        obj.t2_1_div2_name = parameters.Name;
                        obj.inserted_by = parameters.InsertedBy;
                        obj.inserted_date = parameters.InsertedDate;

                        Divisions.Add(obj);
                    }
                }

            }
            data["items"] = Divisions;
            return (data, StatusUtilities.SUCCESS);
        }


    }
}
    
