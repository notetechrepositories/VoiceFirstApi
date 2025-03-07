using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IIssueRepo
    {
        Task<int> AddAsync(object parameters);
        //Task<IEnumerable<IssueModel>> GetAllAsync(Dictionary<string, string> filters);
        Task<string> AddFile(IFormFile formFile);
        Task<IssueModel> GetByIdAsync(string issueId);
        Task<IEnumerable<IssueModel>> GetAllAsync();

    }
}

