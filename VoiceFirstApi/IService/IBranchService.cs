using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IBranchService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(BranchDtoModel Branch);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateBranchDtoModel Branch);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel);
    }
}