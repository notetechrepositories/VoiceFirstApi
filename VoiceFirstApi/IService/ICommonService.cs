using VoiceFirstApi.Models;

namespace VoiceFirstApi.IService
{
    public interface ICommonService
    {
        Task<(Dictionary<string, object>, string, int)> importDivisions(List<ImportDivisionThreeModel> DivisionThreelist);
    }
}
