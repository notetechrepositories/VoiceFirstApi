using System.Collections.Generic;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Service
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepo _SectionRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly ICompanyRepo _companyRepo;
        private readonly IBranchRepo _BranchRepo;
        public SectionService(ISectionRepo SectionRepo, 
            IHttpContextAccessor httpContextAccessor,
                        IBranchRepo BranchRepo,
                   ICompanyRepo companyRepo
            )
        {
            _SectionRepo = SectionRepo;
            _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _BranchRepo = BranchRepo;
            _companyRepo = companyRepo;
        }

        private string GetCurrentUserId()
        {
            if (_HttpContextAccessor == null)
            {
                throw new InvalidOperationException("HTTP Context Accessor is not initialized.");
            }

            // Validate that the HTTP context and user claims are available
            var userClaims = _HttpContextAccessor.HttpContext?.User;
            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Find the user_id claim
            var userIdClaim = userClaims.FindFirst("user_id");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in the token.");
            }
            var decryUserId = SecurityUtilities.Decryption(userIdClaim.Value);
            if (decryUserId == null)
            {
                throw new UnauthorizedAccessException("User ID not found in the token.");
            }
            return decryUserId;
        }
        public async Task<(Dictionary<string, object>, string, int)> AddAsync(InsertSectionDTOModel insert)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "section_name",insert.section_name },
                    { "id_t2_company_branch",insert.id_t2_company_branch }
            };
            var selectionList = _SectionRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (selectionList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                SectionName = insert.section_name.Trim(),
                CompanyBranchId = insert.id_t2_company_branch.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _SectionRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _SectionRepo.DeleteAsync(id);
            if (list > 0)
            {
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
            var list = await _SectionRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }
        public async Task<(Dictionary<string, object>, string, int)> GetAllSectionAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            List<SectionModelWithBranch> sectionWithBranchList = new List<SectionModelWithBranch>();
            var list = await _SectionRepo.GetAllAsync(filters);

            if (list != null)
            {
                foreach (var item in list)
                {
                    SectionModelWithBranch sectionWithBranch = new SectionModelWithBranch();
                    sectionWithBranch.id_t2_company_branch = item.id_t2_company_branch;
                    sectionWithBranch.id_t3_branch_section = item.id_t3_branch_section;
                    sectionWithBranch.t4_selection_name = item.t4_selection_name;

                    Dictionary<string, string> branchfilter = new Dictionary<string, string>
                    {
                        {"id_t2_company_branch",item.id_t2_company_branch }
                    };
                    var branchDetails = await _BranchRepo.GetAllAsync(branchfilter);
                    if (branchDetails.Count() > 0)
                    {
                        sectionWithBranch.branch_details = branchDetails.FirstOrDefault();
                        Dictionary<string, string> companyfilter = new Dictionary<string, string>
                        {
                            {"id_t1_company",branchDetails.FirstOrDefault().id_t1_company }
                        };
                        var companyDetails = await _companyRepo.GetAllAsync(companyfilter);
                        if (companyDetails.Count() > 0)
                        {
                            sectionWithBranch.company_details = companyDetails.FirstOrDefault();
                            sectionWithBranchList.Add(sectionWithBranch);
                        }

                    }




                }
                data["Items"] = sectionWithBranchList;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            data["Items"] = sectionWithBranchList;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);

        }
        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _SectionRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateSectionDTOModel update)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "section_name",update.section_name },
                    { "id_t2_company_branch",update.id_t2_company_branch }
            };
            var selectionList = _SectionRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if (selectionList != null && selectionList.id_t3_branch_section != update.id_t3_branch_section)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = update.id_t3_branch_section,
                SectionName = update.section_name.Trim(),
                CompanyBranchId = update.id_t2_company_branch.Trim(),
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _SectionRepo.UpdateAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string, int)> UpdateStatusAsync(UpdateStatusDtoModel updateStatusDtoModel)
        {
            var data = new Dictionary<string, object>();
            var list = await _SectionRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            };
        }
    }
}
