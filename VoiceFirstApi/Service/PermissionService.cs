using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security;
using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepo _PermissionRepo;
        private readonly IRoleRepo _RoleRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public PermissionService(IPermissionRepo PermissionRepo, IRoleRepo RoleRepo, IHttpContextAccessor httpContextAccessor)
        {
            _PermissionRepo = PermissionRepo;
            _RoleRepo = RoleRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(PermissionDtoModel PermissionDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t5_1_m_user_roles",PermissionDtoModel.id_t5_1_m_user_roles },
                    {"id_t6_link_program_with_program_action",PermissionDtoModel.permission }
            };
            var PermissionList = _PermissionRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (PermissionList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();
            

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = PermissionDtoModel.permission.Trim(),
                RoleId = PermissionDtoModel.id_t5_1_m_user_roles.Trim(),
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _PermissionRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> UpdateAsync(UpdatePermissionDtoModel Permission)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t5_1_m_user_roles",Permission.id_t5_1_m_user_roles },
                    {"id_t6_link_program_with_program_action",Permission.permission }
            };
            var PermissionList = _PermissionRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (PermissionList != null && PermissionList.id_t5_1_m_user_roles_permission!=Permission.id_t5_1_m_user_roles_permission)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = Permission.id_t5_1_m_user_roles_permission,
                Name = Permission.permission,
                RoleId = Permission.id_t5_1_m_user_roles,
                IsDelete = Permission.is_delete,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _PermissionRepo.UpdateAsync(parameters);

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
            var list = await _PermissionRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }
        
        public async Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _PermissionRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _PermissionRepo.DeleteAsync(id);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }
        public async Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            var data = new Dictionary<string, object>();
            var list = await _PermissionRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
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