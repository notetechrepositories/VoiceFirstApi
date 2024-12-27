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
}
