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

        public DivisionThreeService(IDivisionThreeRepo DivisionThreeRepo)
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

        public async Task<(Dictionary<string, object>, string)> AddAsync(DivisionThreeDtoModel DivisionThreeDtoModel)
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
                return (data, StatusUtilities.ALREADY_EXIST);
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
                return (data, StatusUtilities.SUCCESS);
            }
            else
            {
                return (data, StatusUtilities.FAILED);
            }
        }

        public async Task<(Dictionary<string, object>, string)> UpdateAsync(UpdateDivisionThreeDtoModel DivisionThree)
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
                return (data, StatusUtilities.ALREADY_EXIST);
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
            var list = await _DivisionThreeRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionThreeRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS);
        }

        public async Task<(Dictionary<string, object>, string)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _DivisionThreeRepo.DeleteAsync(id);
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