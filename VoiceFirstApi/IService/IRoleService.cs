﻿using VoiceFirstApi.DtoModels;

namespace VoiceFirstApi.IService
{
    public interface IRoleService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(RoleDtoModel Role);
        Task<(Dictionary<string, object>, string,int)> AddRoleWithPermissionAsync(InsertRoleWithPermissionDTOModel Role);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateRoleDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> GetByRoleIdAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateRoleWithPermissionAsync(UpdateRoleWithPermissionDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync();
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel);
        Task<(Dictionary<string, object>, string, int)> GetAllProgramWithActions();


        //------------------------------------ NeW Sys Role-------------------------------------


        Task<(Dictionary<string, object>, string, int)> AddSysRoleAsync(SysRoleDtoModel Role);
        //Task<(Dictionary<string, object>, string, int)> ListAddSysRoleAsync(List<SysRoleDtoModel> Role);
        Task<(Dictionary<string, object>, string, int)> UpdateSysRoleAsync(UpdateSysRoleDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> GetAllSysRoleAsync();
        Task<(Dictionary<string, object>, string, int)> DeleteSysRoleAsync(string id);

        //------------------------------------ NeW Company Role-------------------------------------


        Task<(Dictionary<string, object>, string, int)> AddCompanyRoleAsync(CompanyRoleDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> ListOfAddCompanyRoleAsync(List<CompanyRoleDtoModel> Role);
        Task<(Dictionary<string, object>, string, int)> UpdateCompanyRoleAsync(UpdateCompanyRoleDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> GetAllCompanyRoleAsync();
        Task<(Dictionary<string, object>, string, int)> DeleteCompanyRoleAsync(string id);
    }

}