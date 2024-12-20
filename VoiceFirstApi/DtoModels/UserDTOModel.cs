using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class UserDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string t5_first_name { get; set; }
        public string? t5_last_name { get; set; }
        public string t5_address_1 { get; set; }
        public string? t5_address_2 { get; set; }
        public string t5_zip_code { get; set; }
        public string? t5_mobile_no { get; set; }
        public string? t5_email { get; set; }
        public string t5_birth_year { get; set; }
        public string t5_sex { get; set; }
        public string id_t2_1_local { get; set; }
    }
    public class UpdateUserDtoModel : UserDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_users { get; set; }

    }
}