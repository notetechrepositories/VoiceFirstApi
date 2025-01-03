using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IPermissionService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(PermissionDtoModel Permission);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdatePermissionDtoModel Permission);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel);
    }
}