using VoiceFirstApi.DtoModels;

namespace VoiceFirstApi.IService
{
    public interface IAuthService
    {
        Task<(Dictionary<string, object>, string, int)> AuthLogin(AuthDtoModel authDtoModel);
        Task<(Dictionary<string, object>, string, int)> ForgotPassword(string userName);
        Task<(Dictionary<string, object>, string, int)> VerificationOTP(VerificationOtpDtoModel verificationOtpDtoModel);
        Task<(Dictionary<string, object>, string, int)> ResetPassword(ResetPasswordDtoModel resetPasswordDtoModel);

    }
}
