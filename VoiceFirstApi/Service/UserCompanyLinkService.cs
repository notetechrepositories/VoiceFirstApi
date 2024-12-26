using System.Data;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
using static System.Collections.Specialized.BitVector32;
namespace VoiceFirstApi.Service
{
    public class UserCompanyLinkService : IUserCompanyLinkService
    {
        private readonly IUserCompanyLinkRepo _UserCompanyLinkRepo;

        public UserCompanyLinkService(IUserCompanyLinkRepo UserCompanyLinkRepo)
        {
            _UserCompanyLinkRepo = UserCompanyLinkRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(UserCompanyLinkDtoModel UserCompanyLinkDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t5_users",UserCompanyLinkDtoModel.id_t5_users }
            };
            var UserCopmanyList = _UserCompanyLinkRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (UserCopmanyList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();
            

            var parameters = new
            {
                Id = generatedId.Trim(),
                UserId = UserCompanyLinkDtoModel.id_t5_users.Trim(),
                TypeId = UserCompanyLinkDtoModel.t5_1_m_type_id.Trim(),
                SelectionValueId = UserCompanyLinkDtoModel.id_t4_1_selection_values.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _UserCompanyLinkRepo.AddAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string,int)> UpdateAsync(UpdateUserCompanyLinkDtoModel UserCompanyLink)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t5_users",UserCompanyLink.id_t5_users }
            };
            var UserCopmanyList = _UserCompanyLinkRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if(UserCopmanyList != null && UserCompanyLink.id_t5_2_company_location_users_link!=UserCompanyLink.id_t5_2_company_location_users_link) 
            {
                return(data,StatusUtilities.ALREADY_EXIST,StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = UserCompanyLink.id_t5_2_company_location_users_link,
                UserId = UserCompanyLink.id_t5_users.Trim(),
                TypeId = UserCompanyLink.t5_1_m_type_id.Trim(),
                SelectionValueId = UserCompanyLink.id_t4_1_selection_values.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _UserCompanyLinkRepo.UpdateAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserCompanyLinkRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserCompanyLinkRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _UserCompanyLinkRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }
    }
}