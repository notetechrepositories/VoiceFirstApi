﻿using System.Security.Claims;
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
        private readonly IHttpContextAccessor _HttpContextAccessor;

        private readonly ILocalRepo _LocalRepo;
        public BranchService(IBranchRepo BranchRepo, ILocalRepo localRepo, IHttpContextAccessor httpContextAccessor)
        {
            _BranchRepo = BranchRepo;
            _LocalRepo = localRepo;
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
            var generatedLocalId = Guid.NewGuid().ToString();
            var parametersLocal = new
            {
                Id = generatedLocalId.Trim(),
                CountryId = BranchDtoModel.id_t2_1_country.Trim(),
                Division1Id = BranchDtoModel.id_t2_1_div1.Trim(),
                Division2Id = BranchDtoModel.id_t2_1_div2.Trim(),
                Division3Id = BranchDtoModel.id_t2_1_div3.Trim(),
                Name = BranchDtoModel.t2_1_local_name.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var statusLocal = await _LocalRepo.AddAsync(parametersLocal);
            if (statusLocal == 0)
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
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
                Local= generatedLocalId.Trim(),
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
                await _LocalRepo.DeleteAsync(parameters.Id);
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
                CompanyId = Branch.id_t1_company?.Trim(),
                Name = Branch.t2_company_branch_name?.Trim(),
                BranchType = Branch.t2_id_branch_type?.Trim(),
                Address1 = Branch.t2_address_1?.Trim(),
                Address2 = Branch.t2_address_2?.Trim(),
                ZipCode = Branch.t2_zip_code?.Trim(),
                Mobile = Branch.t2_mobile_no?.Trim(),
                PhoneNo = Branch.t2_phone_no?.Trim(),
                Email = Branch.t2_email?.Trim(),
                Local = Branch.id_t2_1_local?.Trim(), // Ensure 'Local' exists
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            if (string.IsNullOrEmpty(parameters.Local))  // If Local is null or empty, create a new one
            {
                var generatedLocalId = Guid.NewGuid().ToString();

                var parametersLocal = new
                {
                    Id = generatedLocalId,
                    CountryId = Branch.id_t2_1_country?.Trim(),
                    Division1Id = Branch.id_t2_1_div1?.Trim(),
                    Division2Id = Branch.id_t2_1_div2?.Trim(),
                    Division3Id = Branch.id_t2_1_div3?.Trim(),
                    Name = Branch.t2_1_local_name?.Trim(),
                    InsertedBy = userId,
                    InsertedDate = DateTime.UtcNow
                };

                var statusLocal = await _LocalRepo.AddAsync(parametersLocal);
                if (statusLocal == 0)
                {
                    return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
                }

                // Since anonymous types are immutable, create a new object with updated Local value
                parameters = new
                {
                    parameters.Id,
                    parameters.CompanyId,
                    parameters.Name,
                    parameters.BranchType,
                    parameters.Address1,
                    parameters.Address2,
                    parameters.ZipCode,
                    parameters.Mobile,
                    parameters.PhoneNo,
                    parameters.Email,
                    Local = generatedLocalId, // Update Local field
                    parameters.UpdatedBy,
                    parameters.UpdatedDate
                };
            }



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
        public async Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            var data = new Dictionary<string, object>();
            var list = await _BranchRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
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