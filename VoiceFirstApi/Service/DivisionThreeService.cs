using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class DivisionThreeService : IDivisionThreeService
    {
        private readonly IDivisionThreeRepo _DivisionThreeRepo;
        private readonly IDivisionTwoRepo _DivisionTwoRepo;

        public DivisionThreeService(IDivisionThreeRepo DivisionThreeRepo,IDivisionTwoRepo _DivisionTwoRepo)
        {
            _DivisionThreeRepo = DivisionThreeRepo;
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

        public async Task<(Dictionary<string, object>, string, int)> AddAsync(DivisionThreeDtoModel DivisionThreeDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_div2", DivisionThreeDtoModel.id_t2_1_div2 },
                    { "t2_1_div3_name", DivisionThreeDtoModel.t2_1_div3_name }
            };

            var exsitList = _DivisionThreeRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST,StatusUtilities.ALREADY_EXIST_CODE);
            }

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = DivisionThreeDtoModel.t2_1_div3_name.Trim(),
                Div2Id = DivisionThreeDtoModel.id_t2_1_div2.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _DivisionThreeRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateDivisionThreeDtoModel DivisionThree)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t2_1_div2", DivisionThree.id_t2_1_div2 },
                    { "t2_1_div3_name", DivisionThree.t2_1_div3_name }
            };

            var exsitList = _DivisionThreeRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (exsitList != null && exsitList.id_t2_1_div3!=DivisionThree.id_t2_1_div3)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
           
            var parameters = new
            {
                Id = DivisionThree.id_t2_1_div3,
                Div2Id = DivisionThree.id_t2_1_div2,
                Name = DivisionThree.t2_1_div3_name,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _DivisionThreeRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionThreeRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionThreeRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionThreeRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> ImportDivisionThree(List<ImportDivisionThreeModel> DivisionThreelist)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var Divisions = new List<DivisionThreeModel>();
            foreach (var division in DivisionThreelist)
            {
                if (division.t2_1_div2_name != null)  
                {
                    var filter = new Dictionary<string, string>
                    {
                        { "t2_1_div2_name", division.t2_1_div2_name }
                    };
                    var DivisionList = _DivisionTwoRepo.GetAllAsync(filter).Result.FirstOrDefault();

                    if (DivisionList != null)
                    {
                        return (data, StatusUtilities.DIVISION_TWO_NOT_EXSISTS,StatusUtilities.NOT_FOUND_CODE);
                    }
                    else
                    {
                        division.t2_1_div2_name = DivisionList.id_t2_1_div2;
                    }

                }
                else
                {
                    return (data, StatusUtilities.DIVISION_TWO_NOT_EXSISTS, StatusUtilities.NOT_FOUND_CODE);
                }


            }
            foreach (var division in DivisionThreelist)
            {
                var generatedId = Guid.NewGuid().ToString();
                var filter = new Dictionary<string, string>
                {
                    { "id_t2_1_div2", division.t2_1_div2_name},
                    { "t2_1_div3_name", division.t2_1_div3_name }
                };

                var exsitList = _DivisionThreeRepo.GetAllAsync(filter).Result.FirstOrDefault();

                if (exsitList == null)
                {
                    var parameters = new
                    {
                        Id = generatedId.Trim(),
                        Name = division.t2_1_div3_name.Trim(),
                        DivisionTwoId = division.t2_1_div2_name.Trim(),
                        InsertedBy = userId.Trim(),
                        InsertedDate = DateTime.UtcNow
                    };

                    var status = await _DivisionThreeRepo.AddAsync(parameters);

                    if (status > 0)
                    {
                        DivisionThreeModel obj = new DivisionThreeModel();
                        obj.id_t2_1_div2 = parameters.DivisionTwoId;
                        obj.id_t2_1_div3 = parameters.Id;
                        obj.t2_1_div3_name = parameters.Name;
                        obj.inserted_by = parameters.InsertedBy;
                        obj.inserted_date = parameters.InsertedDate;

                        Divisions.Add(obj);
                    }
                }

            }
            data["items"] = Divisions;
            return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
        }

    }
}
