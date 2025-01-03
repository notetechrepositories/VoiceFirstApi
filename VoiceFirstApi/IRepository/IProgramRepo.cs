using VoiceFirstApi.Models;

namespace VoiceFirstApi.IRepository
{
    public interface IProgramRepo
    {
        Task<IEnumerable<ProgramModel>> GetAllPrograms(Dictionary<string, string> filters);
        Task<IEnumerable<GetAllActionWithProgramId>> GetAllProgramLinkWithAction(Dictionary<string, string> filters);

    }

}
