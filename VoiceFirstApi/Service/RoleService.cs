﻿using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Repository;
using VoiceFirstApi.Utilities;
namespace VoiceFirstApi.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _RoleRepo;
        private readonly IPermissionRepo _PermissionRepo;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IProgramRepo _ProgramRepo;
        private readonly IUserCompanyLinkRepo _userCompanyLinkRepo;
        public RoleService(IRoleRepo RoleRepo, IPermissionRepo PermissionRepo, IHttpContextAccessor httpContextAccessor, IProgramRepo programRepo, IUserCompanyLinkRepo userCompanyLinkRepo)
        {
            _RoleRepo = RoleRepo;
            _PermissionRepo = PermissionRepo;
            _HttpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _ProgramRepo = programRepo;
            _userCompanyLinkRepo = userCompanyLinkRepo;
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
        public async Task<(Dictionary<string, object>, string, int)> UpdateRoleWithPermissionAsync(UpdateRoleWithPermissionDtoModel Role)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t5_1_m_user_roles_name",Role.t5_1_m_user_roles_name },
                    { "is_delete", "0" }
            };
            var RoleList = _RoleRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (RoleList != null && RoleList.id_t5_1_m_user_roles != Role.id_t5_1_m_user_roles)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = Role.id_t5_1_m_user_roles.Trim(),
                Name = Role.t5_1_m_user_roles_name.Trim(),
                AllLocationAccess = Role.t5_1_m_all_location_access,
                AllLocationType = Role.t5_1_m_all_location_type,
                OnlyAssignedLocation = Role.t5_1_m_only_assigned_location,
                SelectionValue = Role.id_t4_1_selection_values,
                TypeId = Role.t5_1_m_type_id,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _RoleRepo.UpdateAsync(parameters);
            if (status >0)
            {
                await _PermissionRepo.DeleteByRoleIdAsync(Role.id_t5_1_m_user_roles);
                foreach (var item in Role.Permissions)
                {

                    var filterPermissions = new Dictionary<string, string>
                    {
                            { "id_t5_1_m_user_roles",Role.id_t5_1_m_user_roles },
                            {"id_t6_link_program_with_program_action",item },
                            { "is_delete", "0" }
                    };
                    var PermissionList = _PermissionRepo.GetAllAsync(filterPermissions).Result.FirstOrDefault();
                    if (PermissionList != null)
                    {

                        var parametersPermissions = new
                        {
                            Id = PermissionList.id_t5_1_m_user_roles_permission.Trim(),
                            Name = item.Trim(),
                            RoleId = Role.id_t5_1_m_user_roles.Trim(),
                            IsDelete = 0,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.UtcNow
                        };
                        await _PermissionRepo.UpdateAsync(parametersPermissions);
                    }
                    else
                    {
                        var generatedId = Guid.NewGuid().ToString();
                        var parametersPermissions = new
                        {
                            Id = generatedId.Trim(),
                            Name = item.Trim(),
                            RoleId = Role.id_t5_1_m_user_roles.Trim(),
                            InsertedBy = userId.Trim(),
                            InsertedDate = DateTime.UtcNow
                        };
                        await _PermissionRepo.AddAsync(parametersPermissions);
                    }
                }
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }
        public async Task<(Dictionary<string, object>, string, int)> AddRoleWithPermissionAsync(InsertRoleWithPermissionDTOModel Role)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t5_1_m_user_roles_name",Role.t5_1_m_user_roles_name },
                { "is_delete", "0" }
            };
            var RoleList = _RoleRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (RoleList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = Role.t5_1_m_user_roles_name.Trim(),
                AllLocationAccess = Role.t5_1_m_all_location_access,
                AllLocationType = Role.t5_1_m_all_location_type,
                OnlyAssignedLocation = Role.t5_1_m_only_assigned_location,
                SelectionValue = Role.id_t4_1_selection_values,
                TypeId = Role.t5_1_m_type_id,
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _RoleRepo.AddAsync(parameters);
            if (status >0) 
            {
               
                foreach (var item in Role.Permissions)
                {
                    var Permissionsfilters = new Dictionary<string, string>
                    {
                            { "id_t5_1_m_user_roles",generatedId },
                            {"id_t6_link_program_with_program_action",item },
                            { "is_delete", "0" }
                    };
                    var PermissionList = _PermissionRepo.GetAllAsync(Permissionsfilters).Result.FirstOrDefault();
                    if (PermissionList != null)
                    {
                        var Permissionsparameters = new
                        {
                            Id = PermissionList.id_t5_1_m_user_roles_permission.Trim(),
                            Name = item.Trim(),
                            RoleId = generatedId.Trim(),
                            IsDelete = 0,
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.UtcNow
                        };
                        await _PermissionRepo.UpdateAsync(Permissionsparameters);
                    }
                    else
                    {
                        var generatedPermissionsId = Guid.NewGuid().ToString();
                        var Permissionsparameters = new
                        {
                            Id = generatedPermissionsId.Trim(),
                            Name = item.Trim(),
                            RoleId = generatedId.Trim(),
                            InsertedBy = userId.Trim(),
                            InsertedDate = DateTime.UtcNow
                        };
                        await _PermissionRepo.AddAsync(Permissionsparameters);
                    }
                }
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }
        public async Task<(Dictionary<string, object>, string,int)> AddAsync(RoleDtoModel RoleDtoModel)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t5_1_m_user_roles_name",RoleDtoModel.t5_1_m_user_roles_name },
                    { "is_delete", "0" }
            };
            var RoleList = _RoleRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (RoleList != null)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var generatedId = Guid.NewGuid().ToString();

            var parameters = new
            {
                Id = generatedId.Trim(),
                Name = RoleDtoModel.t5_1_m_user_roles_name.Trim(),
                AllLocationAccess=RoleDtoModel.t5_1_m_all_location_access,
                AllLocationType=RoleDtoModel.t5_1_m_all_location_type,
                OnlyAssignedLocation=RoleDtoModel.t5_1_m_only_assigned_location,
                SelectionValue = RoleDtoModel.id_t4_1_selection_values,
                TypeId = RoleDtoModel.t5_1_m_type_id,
                InsertedBy = userId.Trim(),
                InsertedDate = DateTime.UtcNow
            };

            var status = await _RoleRepo.AddAsync(parameters);

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

        public async Task<(Dictionary<string, object>, string,int)> UpdateAsync(UpdateRoleDtoModel Role)
        {
            var userId = GetCurrentUserId();
            var data = new Dictionary<string, object>();
            var filter = new Dictionary<string, string>
            {
                    { "t5_1_m_user_roles_name",Role.t5_1_m_user_roles_name }
            };
            var RoleList = _RoleRepo.GetAllAsync(filter).Result.FirstOrDefault();

            if (RoleList != null && RoleList.id_t5_1_m_user_roles!=Role.id_t5_1_m_user_roles)
            {
                return (data, StatusUtilities.ALREADY_EXIST, StatusUtilities.ALREADY_EXIST_CODE);
            }
            var parameters = new
            {
                Id = Role.id_t5_1_m_user_roles.Trim(),
                Name = Role.t5_1_m_user_roles_name.Trim(),
                AllLocationAccess = Role.t5_1_m_all_location_access,
                AllLocationType = Role.t5_1_m_all_location_type,
                OnlyAssignedLocation = Role.t5_1_m_only_assigned_location,
                SelectionValue = Role.id_t4_1_selection_values,
                TypeId = Role.t5_1_m_type_id,
                UpdatedBy = userId,
                UpdatedDate = DateTime.UtcNow
            };

            var status = await _RoleRepo.UpdateAsync(parameters);

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
        public async Task<(Dictionary<string, object>, string, int)> GetByRoleIdAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var filterRole = new Dictionary<string, string>
            {
                { "is_delete", "0" }
            };
            var Role = _RoleRepo.GetByIdAsync(id, filterRole).Result;

            data["Role"] = Role;
            var filter = new Dictionary<string, string>
            {
                { "id_t5_1_m_user_roles", id },
                { "is_delete", "0" }
            };
            var PermissionList = _PermissionRepo.GetAllAsync(filter);
            if (PermissionList?.Result != null && PermissionList.Result.Any())
            {
                // Create a list to hold the permissions
                var permissionList = PermissionList.Result.Select(permission => permission.id_t6_link_program_with_program_action).ToList();

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
        public async Task<(Dictionary<string, object>, string,int)> GetAllAsync()
        {
            var UserId = GetCurrentUserId();
            var filter = new Dictionary<string, string>
            {
                {"id_t5_users",UserId },
                {"is_delete","0" }
            };
            
            var UserCompanyDetails = await _userCompanyLinkRepo.GetAllAsync( filter);
            var UserCompanyOrBranchId = UserCompanyDetails.FirstOrDefault().id_t4_1_selection_values;
            var filters = new Dictionary<string, string>
            {
                {"id_t4_1_selection_values",UserCompanyOrBranchId }
            };   
            var data = new Dictionary<string, object>();
            var list = await _RoleRepo.GetAllAsync(filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> GetByIdAsync(string id, Dictionary<string, string> filters)
        {
            var data = new Dictionary<string, object>();
            var list = await _RoleRepo.GetByIdAsync(id, filters);
            data["Items"] = list;
            return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
        }

        public async Task<(Dictionary<string, object>, string,int)> DeleteAsync(string id)
        {
            var data = new Dictionary<string, object>();
            var list = await _RoleRepo.DeleteAsync(id);
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
            var list = await _RoleRepo.UpdateStatus(updateStatusDtoModel.id, updateStatusDtoModel.status);
            if (list > 0)
            {
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> GetAllProgramWithActions()
        {
            var data = new Dictionary<string, object>();
            List<GetAllProgramWithActions> getAllProgramWithActions = new List<GetAllProgramWithActions>();
            var filter= new Dictionary<string, string>
            {
                
            };
            var list = await _ProgramRepo.GetAllPrograms(filter);
            if (list != null)
            {
                foreach (var value in list)
                {
                    GetAllProgramWithActions obj = new GetAllProgramWithActions();
                    obj.t6_program_name = value.t6_program_name;
                    var id = new Dictionary<string, string>
                    {
                        { "id_t6_program",value.id_t6_program}
                    };
                    var actions = await _ProgramRepo.GetAllProgramLinkWithAction(id);
                    
                    if (actions != null)
                    {
                        List<ProgramActions> actionsList = new List<ProgramActions>();
                        foreach(var action in actions)
                        {
                            ProgramActions objects= new ProgramActions();
                            objects.t6_action = action.t6_action;
                            objects.id_t6_link_program_with_program_action = action.id_t6_link_program_with_program_action;
                            actionsList.Add(objects);
                        }
                        obj.programActions = actionsList;
                        getAllProgramWithActions.Add(obj);
                    }
                    else
                    {
                        return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
                    }

                }
                data["Items"] = getAllProgramWithActions;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);

            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }

            
        }
    }
}