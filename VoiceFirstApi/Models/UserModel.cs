namespace VoiceFirstApi.Models
{
    public class UserModel
    {
        public string id_t5_users { get; set; }
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
        public string id_t5_1_m_user_roles { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
    public class UserDetailsModel:UserModel
    {
        public string t5_password { get; set; }
    }
}