using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IUserCompanyLinkService
    {
        Task<(Dictionary<string, object>, string,int)> AddAsync(UserCompanyLinkDtoModel UserCompanyLink);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateUserCompanyLinkDtoModel UserCompanyLink);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
    }
}