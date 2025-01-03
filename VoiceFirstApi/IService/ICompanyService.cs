using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ICompanyService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(CompanyDtoModel Company);
        Task<(Dictionary<string, object>, string,int)> InsertCompany(InsertCompanyDTOModel Company);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateCompanyDtoModel Company);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel);
    }
}