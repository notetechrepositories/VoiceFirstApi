using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace VoiceFirstApi.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepo _CompanyRepo;
        private readonly IBranchRepo _BranchRepo;

        public CompanyService(ICompanyRepo CompanyRepo, IBranchRepo branchRepo)
        {
            _CompanyRepo = CompanyRepo;
            _BranchRepo = branchRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(CompanyDtoModel CompanyDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t1_company_name", CompanyDtoModel.t1_company_name }
            };
            var companyList= _CompanyRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (companyList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = CompanyDtoModel.t1_company_name.Trim(),
                Type= CompanyDtoModel.id_company_type.Trim(),
                Currency=CompanyDtoModel.id_currency.Trim(),
                Date=CompanyDtoModel.is_active_till_date,
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _CompanyRepo.AddAsync(parameters);

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
        public async Task<(Dictionary<string, object>, string, int)> InsertCompany(InsertCompanyDTOModel Company)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var repoStatus = 0;
            var filter = new Dictionary<string, string>
            {
                    { "t1_company_name", Company.t1_company_name }
            };
            var companyList = _CompanyRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (companyList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = Company.t1_company_name.Trim(),
                Type = Company.id_company_type.Trim(),
                Currency = Company.id_currency.Trim(),
                Date = Company.is_active_till_date,
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };
            var status = await _CompanyRepo.AddAsync(parameters);

            if (status > 0)
            {
                var filterBranch = new Dictionary<string, string>
                {
                        { "t2_company_branch_name", Company.insertBranchDTOModel.t2_company_branch_name }
                };
                var BranchList = _BranchRepo.GetAllAsync(filterBranch).Result.FirstOrDefault();
                if (BranchList != null)
                {
                    return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
                }
                var generatedBranchId = Guid.NewGuid().ToString();

                var parametersBranch = new
                {
                    Id = generatedBranchId.Trim(),
                    CompanyId = parameters.Id.Trim(),
                    Name = Company.insertBranchDTOModel.t2_company_branch_name.Trim(),
                    BranchType = Company.insertBranchDTOModel.t2_id_branch_type.Trim(),
                    Address1 = Company.insertBranchDTOModel.t2_address_1.Trim(),
                    Address2 = Company.insertBranchDTOModel.t2_address_2.Trim(),
                    ZipCode = Company.insertBranchDTOModel.t2_zip_code.Trim(),
                    Mobile = Company.insertBranchDTOModel.t2_mobile_no.Trim(),
                    PhoneNo = Company.insertBranchDTOModel.t2_phone_no.Trim(),
                    Email = Company.insertBranchDTOModel.t2_email.Trim(),
                    Local = Company.insertBranchDTOModel.id_t2_1_local.Trim(),
                    InsertedBy = userId.Trim(),
                    InsertedDate = DateTime.UtcNow
                };

                var branchStatus= _BranchRepo.AddAsync(parametersBranch);


            }
            return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateCompanyDtoModel Company)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var parameters = new
            {
                Id = Company.id_t1_company.Trim(),
                Name = Company.t1_company_name.Trim(),
                Type = Company.id_company_type.Trim(),
                Currency = Company.id_currency.Trim(),
                Date = Company.is_active_till_date,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _CompanyRepo.UpdateAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED,StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CompanyRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _CompanyRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _CompanyRepo.DeleteAsync(id);
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