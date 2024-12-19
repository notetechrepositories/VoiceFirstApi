using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace VoiceFirstApi.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepo _PermissionRepo;

        public PermissionService(IPermissionRepo PermissionRepo)
        {
            _PermissionRepo = PermissionRepo;
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

        public async Task<(Dictionary<string, object>, string,int)> AddAsync(PermissionDtoModel PermissionDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "id_t5_1_m_user_roles",PermissionDtoModel.id_t5_1_m_user_roles },
                    {"permission",PermissionDtoModel.permission }
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
                    {"permission",Permission.permission }
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
        public async Task<(Dictionary<string, object>, string, int)> GetBtRoleIdAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                { "id_t5_1_m_user_roles", id },
                { "is_delete", "0" }
            };

            var PermissionList = _PermissionRepo.GetAllAsync(filter);
            if (PermissionList?.Result != null && PermissionList.Result.Any())
            {
                // Create a list to hold the permissions
                var permissionList = PermissionList.Result.Select(permission => permission.permission).ToList();

                // Add the list to the data dictionary
                data["Items"] = permissionList;
            }
            else
            {
                // Assign an empty list if no permissions are found
                data["Items"] = new List<string>();
            }

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

        
    }
}