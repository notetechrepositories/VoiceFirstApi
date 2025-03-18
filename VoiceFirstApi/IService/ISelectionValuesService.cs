using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ISelectionValuesService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(SelectionValuesDtoModel SelectionValues);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateSelectionValuesDtoModel SelectionValues);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> GetRoleType();
        Task<(Dictionary<string, object>, string, int)> GetCompanyType();
        Task<(Dictionary<string, object>, string, int)> GetBranchType();
        Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel);
    }
}