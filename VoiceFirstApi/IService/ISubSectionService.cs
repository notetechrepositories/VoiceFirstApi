using VoiceFirstApi.DtoModels;

namespace VoiceFirstApi.IService
{
    public interface ISubSectionService
    {
        Task<(Dictionary<string, object>, string, int)> AddAsync(InsertSubSectionDTOModel insert);
        Task<(Dictionary<string, object>, string, int)> UpdateAsync(UpdateSubSectionDTOModel update);
        Task<(Dictionary<string, object>, string, int)> GetAllAsync(Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> GetByIdAsync(string id, Dictionary<string, string> filters);
        Task<(Dictionary<string, object>, string, int)> DeleteAsync(string id);
        Task<(Dictionary<string, object>, string, int)> UpdateStatusAsync(UpdateStatusDtoModel updateStatusDtoModel);
    }
}
