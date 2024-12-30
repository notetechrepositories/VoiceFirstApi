using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VoiceFirstApi.Utilities;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Repository;
using Microsoft.Extensions.Configuration;
using VoiceFirstApi.Models;
using VoiceFirstApi.Utilits;

namespace VoiceFirstApi.Service
{
    public class AuthService : IAuthService
    {
        public readonly IUserRepo _UserRepo;
        public readonly IRoleRepo _RoleRepo;

        private readonly IConfiguration _Configuration;
        public AuthService(IUserRepo UserRepo, IConfiguration Configuration, IRoleRepo roleRepo)
        {
            _UserRepo = UserRepo;
            _Configuration = Configuration;
            _RoleRepo = roleRepo;
        }
        public async Task<(Dictionary<string, object>, string, int)> AuthLogin(AuthDtoModel authDtoModel)
        {
            var data = new Dictionary<string, object>();
            var userDetails = await _UserRepo.GetUserDetailsByEmailOrPhone(authDtoModel.username);
            if(userDetails != null)
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
                    var filter = new Dictionary<string, string>
                    {
                            { "id_t5_1_m_user_roles", userDetails.id_t5_1_m_user_roles }
                    };
                    var roleDeatils = _RoleRepo.GetAllAsync(filter).Result.FirstOrDefault();
                    if(roleDeatils.id_t4_1_selection_values == "6d5b7f76-bae0-44d6-ac6b-52a66ebe786b")
                    {
                        data["role"] = "Notetech";
                    }
                    else
                    {
                        data["role"] = "Comapny";
                    }
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

        public async Task<(Dictionary<string, object>, string, int)> ForgotPassword(string userName)
        {
            var data = new Dictionary<string, object>();
            var userDetails=await _UserRepo.GetUserDetailsByEmailOrPhone(userName);
            if (userDetails != null)
            {
                var otp = StringUtilities.GenerateRandomOTP(6);
                var encrycryptedOtp = SecurityUtilities.Encryption(otp);
                var encrycryptedUserId = SecurityUtilities.Encryption(userDetails.id_t5_users);
                var encryptModel = SecurityUtilities.EncryptModel(encrycryptedOtp, encrycryptedUserId.ToString());
                var emailService = new CommunicationUtilities();
                
                EmailModel mail = new EmailModel();
                mail.from_email_password = "frsj ucpw vaww xzmv";
                mail.from_email = "anil.p@notetech.com";
                mail.to_email = userDetails.t5_email;
                mail.email_html_body =
                    "<html>" +
                        "<body>" +
                            "<p>Hi " + userDetails.t5_first_name + ",</p>" +
                            "<p>Your OTP is: <strong>" + otp + "</strong></p>" +
                            "<p>Please use this code to verify your account. This code will expire in 1 minutes.</p>" +
                            "<p><strong>Thanks & Regards,</strong><br>" +
                            "<em>Notetech Team</em></p>" +
                            "<p><em>Powered by Notetech Software</em></p>" +
                        "</body>" +
                    "</html>";
                mail.subject = "Your OTP Code to Verify Your Account";
                emailService.SendMail(mail);
                data["encryptedOtp"] = encryptModel;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.USER_NOT_FOUND, StatusUtilities.NOT_FOUND_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> ResetPassword(ResetPasswordDtoModel resetPasswordDtoModel)
        {
            var data = new Dictionary<string, object>();
            var decryptedUserId = SecurityUtilities.Decryption(resetPasswordDtoModel.user_id);
            if (decryptedUserId == null)
            {
                return (data, StatusUtilities.UNAUTHORIZED_ACCESS, StatusUtilities.UNAUTHORIZED_ACCESS_CODE);
            }
            var filter = new Dictionary<string, string>
            {
                    { "id_t5_users", decryptedUserId }
            };
            var userDetails = _UserRepo.GetAllAsync(filter).Result.FirstOrDefault();
            if(userDetails == null)
            {
                return (data, StatusUtilities.USER_NOT_FOUND, StatusUtilities.NOT_FOUND_CODE);
            }
            var EncryptPassword = SecurityUtilities.Encryption(resetPasswordDtoModel.password);
            var parameters = new
            {
                Id = decryptedUserId.Trim(),
                Password = EncryptPassword,
                UpdatedBy = decryptedUserId,
                UpdatedDate = DateTime.UtcNow,
            };
            var status = await _UserRepo.UpdatePasswordAsync(parameters);

            if (status > 0)
            {
                data["Items"] = parameters;
                return (data, StatusUtilities.SUCCESS, StatusUtilities.SUCCESS_CODE);
            }
            else
            {
                return (data, StatusUtilities.FAILED, StatusUtilities.FAILED_CODE);
            }
        }

        public async Task<(Dictionary<string, object>, string, int)> VerificationOTP(VerificationOtpDtoModel verificationOtpDtoModel)
        {
            var data = new Dictionary<string, object>();
            var decryptedData = SecurityUtilities.DecryptModel(verificationOtpDtoModel.encrypted_data);


            if(decryptedData == null )
            {
                return (data, StatusUtilities.UNAUTHORIZED_ACCESS, StatusUtilities.UNAUTHORIZED_ACCESS_CODE);
            }
            else
            {

                var currentDate = DateTime.Now;
                var diffTime = currentDate - decryptedData.CurrentDate;

                if (diffTime.TotalMinutes > 1)
                {

                    return (data, StatusUtilities.OTP_EXPIRED, StatusUtilities.FAILED_CODE);
                }
              
                var decryptedOtp = SecurityUtilities.Decryption(decryptedData.value);
                if (decryptedOtp == verificationOtpDtoModel.otp)
                {
                    data["userData"] = decryptedData.userDeatils;
                    return (data,StatusUtilities.SUCCESS,StatusUtilities.SUCCESS_CODE);
                }
                else
                {
                    return (data, StatusUtilities.INVALID_OTP, StatusUtilities.INVALID_OTP_CODE);
                }
            }
        }
    }
}
