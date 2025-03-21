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


        //----------------------------------------------- Sys selection values-------------------------------------------------------

        Task<(Dictionary<string, object>, string, int)> AddSysAsync(SysSelectionValuesDtoModel SelectionValues);
        Task<(Dictionary<string, object>, string, int)> UpdateSysAsync(UpdateSysSelectionValuesDtoModel SelectionValues);
        Task<(Dictionary<string, object>, string, int)> GetAllSysAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteSysAsync(string id);
        Task<(Dictionary<string, object>, string, int)> GetAllSysValuesBySectionTypeAsync(string selectionId);
        Task<(Dictionary<string, object>, string, int)> UpdateSysStatus(UpdateStatusDtoModel updateStatusDtoModel);

        //----------------------------------------------- User selection values -------------------------------------------------------

        Task<(Dictionary<string, object>, string, int)> AddUserAsync(UserSelectionValuesDtoModel SelectionValues);
        Task<(Dictionary<string, object>, string, int)> UpdateUserAsync(UpdateUserSelectionValuesDtoModel SelectionValues);
        Task<(Dictionary<string, object>, string, int)> GetAllUserAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteUserAsync(string id);
        Task<(Dictionary<string, object>, string, int)> GetAllValuesBySectionTypeAsync(string selectionId);
        Task<(Dictionary<string, object>, string, int)> UpdateUserStatus(UpdateStatusDtoModel updateStatusDtoModel);
    }


}