using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class RoleDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string t5_1_m_user_roles_name { get; set; }
        public int t5_1_m_all_location_access { get; set; }
        public int t5_1_m_all_location_type { get; set; }
        public int t5_1_m_only_assigned_location { get; set; }
        public string t5_1_m_type { get; set; }
        public string t5_1_m_type_id { get; set; }
    }
    public class UpdateRoleDtoModel : RoleDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_1_m_user_roles { get; set; }

    }
}