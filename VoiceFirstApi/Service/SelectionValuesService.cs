using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class SelectionValuesService : ISelectionValuesService
    {
        private readonly ISelectionValuesRepo _SelectionValuesRepo;
        private readonly IUserCompanyLinkRepo _UserCompanyLinkRepo;
        private readonly ICompanyRepo _CompanyRepo;
        private readonly IBranchRepo _BranchRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public SelectionValuesService
        (   ISelectionValuesRepo SelectionValuesRepo, 
            IUserCompanyLinkRepo userCompanyLinkRepo,
            ICompanyRepo companyRepo,
            IBranchRepo branchRepo
            , IHttpContextAccessor httpContextAccessor
        )
        {
            _SelectionValuesRepo = SelectionValuesRepo;
            _UserCompanyLinkRepo = userCompanyLinkRepo;
            _CompanyRepo = companyRepo;
            _BranchRepo = branchRepo;
            _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(SelectionValuesDtoModel SelectionValuesDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var generatedId = Guid.NewGuid().ToString();
            var filter = new Dictionary<string, string>
            {
                    { "t4_1_selection_values_name",SelectionValuesDtoModel.t4_1_selection_values_name },
                    { "id_t4_selection",SelectionValuesDtoModel.id_t4_selection }
            };
            var selectionValueList = _SelectionValuesRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (selectionValueList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = generatedId.Trim(),
                SelectionId = SelectionValuesDtoModel.id_t4_selection.Trim(),
                Name = SelectionValuesDtoModel.t4_1_selection_values_name.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _SelectionValuesRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> UpdateAsync(UpdateSelectionValuesDtoModel SelectionValues)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t4_1_selection_values_name",SelectionValues.t4_1_selection_values_name },
                    { "id_t4_selection",SelectionValues.id_t4_selection }
            };
            var selectionValueList = _SelectionValuesRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (selectionValueList != null && selectionValueList.id_t4_1_selection_values!= SelectionValues.id_t4_1_selection_values)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = SelectionValues.id_t4_1_selection_values,
                SelectionId = SelectionValues.id_t4_selection,
                Name = SelectionValues.t4_1_selection_values_name,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _SelectionValuesRepo.UpdateAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> GetAllAsync(Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _SelectionValuesRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _SelectionValuesRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _SelectionValuesRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetRoleType()
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            List<GetRoleTypeModel> getRoleTypeModels = new List<GetRoleTypeModel>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t4_selection","c2e63589-4324-417f-8f26-88c16f346e96"}
            };
            var selectionvalue = await _SelectionValuesRepo.GetAllAsync(filter);
            if(selectionvalue != null)
            {
                foreach (var value in selectionvalue)
                {
                    GetRoleTypeModel obj = new GetRoleTypeModel();
                    obj.id_t4_1_selection_values = value.id_t4_1_selection_values;
                    obj.t4_1_selection_values_name = value.t4_1_selection_values_name;
                    var filters = new Dictionary<string, string>
                {
                            { "id_t5_users",userId},
                };
                    var userCompanyLinkList = _UserCompanyLinkRepo.GetAllAsync(filters).Result.FirstOrDefault();
                    if (userCompanyLinkList != null)
                    {

                        if (value.id_t4_1_selection_values == "35c0c4e0-1a33-4a7f-9705-636cd5f9403f")
                        {
                            if (userCompanyLinkList.id_t4_1_selection_values == "35c0c4e0-1a33-4a7f-9705-636cd5f9403f")
                            {
                                var ComapnyFilter = new Dictionary<string, string>
                            {
                                        { "id_t1_company",userCompanyLinkList.t5_1_m_type_id},
                            };
                                var CompanyData = _CompanyRepo.GetAllAsync(ComapnyFilter).Result.FirstOrDefault();
                                if (CompanyData != null)
                                {
                                    obj.role_type_data = new List<RoleTypeModel>
                                {
                                    new RoleTypeModel
                                    {
                                        type_id = CompanyData.id_t1_company,
                                        type_name = CompanyData.t1_company_name
                                    }
                                };
                                }

                            }
                            if (userCompanyLinkList.id_t4_1_selection_values == "5efb48b2-c6c5-40e7-bafd-94f59bc6cd3f")
                            {
                                var BranchFilter = new Dictionary<string, string>
                            {
                                        { "id_t2_company_branch",userCompanyLinkList.t5_1_m_type_id},
                            };
                                var branchData = _BranchRepo.GetAllAsync(BranchFilter).Result.FirstOrDefault();
                                if (branchData != null)
                                {
                                    var ComapnyFilter = new Dictionary<string, string>
                                {
                                            { "id_t1_company",branchData.id_t1_company},
                                };
                                    var CompanyData = _CompanyRepo.GetAllAsync(ComapnyFilter).Result.FirstOrDefault();
                                    if (CompanyData != null)
                                    {
                                        obj.role_type_data = new List<RoleTypeModel>
                                    {
                                        new RoleTypeModel
                                        {
                                            type_id = CompanyData.id_t1_company,
                                            type_name = CompanyData.t1_company_name
                                        }
                                    };
                                    }

                                }

                            }
                        }
                        if (value.id_t4_1_selection_values == "5efb48b2-c6c5-40e7-bafd-94f59bc6cd3f")
                        {
                            if (userCompanyLinkList.id_t4_1_selection_values == "35c0c4e0-1a33-4a7f-9705-636cd5f9403f")
                            {
                                var ComapnyFilter = new Dictionary<string, string>
                            {
                                        { "id_t1_company",userCompanyLinkList.t5_1_m_type_id},
                            };
                                var BranchDataList = _BranchRepo.GetAllAsync(ComapnyFilter).Result;
                                if (BranchDataList != null)
                                {
                                    List<RoleTypeModel> BranchList = new List<RoleTypeModel>();
                                    foreach (var item in BranchDataList)
                                    {
                                        RoleTypeModel roleTypeModel = new RoleTypeModel();
                                        roleTypeModel.type_id = item.id_t2_company_branch;
                                        roleTypeModel.type_name = item.t2_company_branch_name;
                                        BranchList.Add(roleTypeModel);
                                    }
                                    obj.role_type_data = BranchList;
                                }

                            }
                            if (userCompanyLinkList.id_t4_1_selection_values == "5efb48b2-c6c5-40e7-bafd-94f59bc6cd3f")
                            {
                                var BranchFilter = new Dictionary<string, string>
                            {
                                        { "id_t2_company_branch",userCompanyLinkList.t5_1_m_type_id},
                            };
                                var branchData = _BranchRepo.GetAllAsync(BranchFilter).Result.FirstOrDefault();
                                if (branchData != null)
                                {
                                    var ComapnyFilter = new Dictionary<string, string>
                                {
                                            { "id_t1_company",branchData.id_t1_company},
                                };
                                    var BranchDataList = _BranchRepo.GetAllAsync(ComapnyFilter).Result;
                                    if (BranchDataList != null)
                                    {
                                        List<RoleTypeModel> BranchList = new List<RoleTypeModel>();
                                        foreach (var item in BranchDataList)
                                        {
                                            RoleTypeModel roleTypeModel = new RoleTypeModel();
                                            roleTypeModel.type_id = item.id_t2_company_branch;
                                            roleTypeModel.type_name = item.t2_company_branch_name;
                                            BranchList.Add(roleTypeModel);
                                        }
                                        obj.role_type_data = BranchList;
                                    }

                                }

                            }
                        }
                    }

                    getRoleTypeModels.Add(obj);
                }
                if (getRoleTypeModels.Count > 0)
                {
                    data["Items"] = getRoleTypeModels;
                    return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
                }
                
            }
           return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            
        }
    }
}