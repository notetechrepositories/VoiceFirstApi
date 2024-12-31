using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class AuthDtoModel
    {
        [Required(ErrorMessage = "User name is required.")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string password { get; set; }
    }
    public class VerificationOtpDtoModel

    {
        [Required(ErrorMessage = "OTP is required.")]
        [StringLength(6, ErrorMessage = "OTP must be exactly 6 characters long.")]
        public string otp { get; set; }

        [Required(ErrorMessage = "Encrypted OTP is required.")]
       
        public string encrypted_data { get; set; }
    }
    public class ResetPasswordDtoModel
    {
        [Required(ErrorMessage = "User id is required.")]
        public string user_id { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "password must be at least 6 characters long.")]
        public string password { get; set; }
    }
    public class ChangePasswordDtoModel
    {
        [Required(ErrorMessage = "current password is required.")]
        public string current_password { get; set; }
        [Required(ErrorMessage = "new password is required.")]
        [MinLength(6, ErrorMessage = "password must be at least 6 characters long.")]
        public string password { get; set; }
    }
}
