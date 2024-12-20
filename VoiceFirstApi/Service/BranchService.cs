using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepo _BranchRepo;

        public BranchService(IBranchRepo BranchRepo)
        {
            _BranchRepo = BranchRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(BranchDtoModel BranchDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t2_company_branch_name", BranchDtoModel.t2_company_branch_name }
            };
            var BranchList = _BranchRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (BranchList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                CompanyId=BranchDtoModel.id_t1_company.Trim(),
                Name = BranchDtoModel.t2_company_branch_name.Trim(),
                BranchType=BranchDtoModel.t2_id_branch_type.Trim(),
                Address1=BranchDtoModel.t2_address_1.Trim(),
                Address2=BranchDtoModel.t2_address_2.Trim(),
                ZipCode=BranchDtoModel.t2_zip_code.Trim(),
                Mobile=BranchDtoModel.t2_mobile_no.Trim(),
                PhoneNo=BranchDtoModel.t2_phone_no.Trim(),
                Email=BranchDtoModel.t2_email.Trim(),
                Local=BranchDtoModel.id_t2_1_local.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _BranchRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> UpdateAsync(UpdateBranchDtoModel Branch)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var parameters = new
            {
                Id = Branch.id_t2_company_branch,
                CompanyId = Branch.id_t1_company.Trim(),
                Name = Branch.t2_company_branch_name.Trim(),
                BranchType = Branch.t2_id_branch_type.Trim(),
                Address1 = Branch.t2_address_1.Trim(),
                Address2 = Branch.t2_address_2.Trim(),
                ZipCode = Branch.t2_zip_code.Trim(),
                Mobile = Branch.t2_mobile_no.Trim(),
                PhoneNo = Branch.t2_phone_no.Trim(),
                Email = Branch.t2_email.Trim(),
                Local = Branch.id_t2_1_local.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _BranchRepo.UpdateAsync(parameters);

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
            var list = await _BranchRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _BranchRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _BranchRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }
    }
}