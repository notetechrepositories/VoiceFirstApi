using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class PermissionDtoModel
    {

        public string id_t5_1_m_user_roles { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string permission { get; set; }
    }
    public class UpdatePermissionDtoModel : PermissionDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_1_m_user_roles_permission { get; set; }
        public int is_delete { get; set; }

    }
}