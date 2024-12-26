using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class UserCompanyLinkDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_users { get; set; }
        public string t5_1_m_type_id { get; set; }
        public string id_t4_1_selection_values { get; set; }
    }
    public class UpdateUserCompanyLinkDtoModel : UserCompanyLinkDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_2_company_location_users_link { get; set; }

    }
}