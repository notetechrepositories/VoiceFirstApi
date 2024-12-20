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
        public string? id_t5_1_m_user_roles { get; set; }
        public string id_t2_1_country { get; set; }
        public string id_t2_1_div1 { get; set; }
        public string id_t2_1_div2 { get; set; }
        public string id_t2_1_div3 { get; set; }
        public string t2_1_local_name { get; set; }
    }
    public class UpdateUserDtoModel : UserDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_users { get; set; }
        public string id_t2_1_local { get; set; }

    }
}