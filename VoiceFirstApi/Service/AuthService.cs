using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VoiceFirstApi.Utilities;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Repository;
using Microsoft.Extensions.Configuration;

namespace VoiceFirstApi.Service
{
    public class AuthService : IAuthService
    {
        public readonly IUserRepo _UserRepo;

        private readonly IConfiguration _Configuration;
        public AuthService(IUserRepo UserRepo, IConfiguration Configuration)
        {
            _UserRepo = UserRepo;
            _Configuration= Configuration;
        }
        public async Task<(Dictionary<string, object>, string, int)> AuthLogin(AuthDtoModel authDtoModel)
        {
            var data = new Dictionary<string, object>();
            var userDetails = await _UserRepo.GetUserDetailsByEmailOrPhone(authDtoModel.username);
            +if(userDetails != null)
            {
                var decryptPassword = SecurityUtilities.Decryption(userDetails.t5_password);
                if(decryptPassword != null && decryptPassword ==authDtoModel.password) 
                {
                    var encryptUserId = SecurityUtilities.Encryption(userDetails.id_t5_users);
                    var claimDetails = new Dictionary<string, string>();
                    claimDetails["user_id"] = encryptUserId;
                    var tokenUtilities = new SecurityUtilities(_Configuration);
                    var token = tokenUtilities.GetToken(claimDetails);

                    var users = new 
                    {
                        t5_first_name = userDetails.t5_first_name,
                        t5_last_name = userDetails.t5_last_name,
                        t5_sex = userDetails.t5_sex,
                        t5_email = userDetails.t5_email,
                        t5_mobile_no=userDetails.t5_mobile_no,
                    };

                data["token"] = token;
                data["userDetails"] = users;

                    return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
                    
                }
                else
                {
                    return (data, StatusUtilities.INVALID_PASSWORD, StatusUtilities.UNAUTHORIZED_ACCESS_CODE);
                }
            }
            else
            {
                return (data, StatusUtilities.USER_NOT_FOUND, StatusUtilities.NOT_FOUND_CODE);
            }
            
            
        }
    }
}
