using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface IIssueService
    {
        Task<(Dictionary<string, object>, string, int)> AddAsync(IssueDtoModel Issue);
        Task<(IssueModel?, string, int)> GetByIdAsync(string issueId);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync();
    }
}
