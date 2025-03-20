using VoiceFirstApi.DtoModels;
using VoiceFirstApi.Repository;

namespace VoiceFirstApi.IService
{
    public interface ISectionService
    {
        Task<(Dictionary<string, object>, string, int)> AddAsync(InsertSectionDTOModel insert);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateSectionDTOModel update);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetAllSectionAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatusAsync(UpdateStatusDtoModel updateStatusDtoModel);


    }
}
