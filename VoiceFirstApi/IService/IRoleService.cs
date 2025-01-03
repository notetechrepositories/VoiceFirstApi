using VoiceFirstApi.DtoModels;

namespace VoiceFirstApi.IService
{
    public interface IRoleService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(RoleDtoModel Role);
        Task<(Dictionary<string, object>, string,int)> AddRoleWithPermissionAsync(InsertRoleWithPermissionDTOModel Role);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateRoleDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> GetBtRoleIdAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateRoleWithPermissionAsync(UpdateRoleWithPermissionDtoModel Role);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel);
        Task<(Dictionary<string, object>, string, int)> GetAllProgramWithActions();

    }
    
}