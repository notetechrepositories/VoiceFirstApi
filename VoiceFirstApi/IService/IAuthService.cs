using VoiceFirstApi.DtoModels;

namespace VoiceFirstApi.IService
{
    public interface IAuthService
    {
        Task<(Dictionary<string, object>, string, int)> AuthLogin(AuthDtoModel authDtoModel);
    }
}
