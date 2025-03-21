using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class SelectionValuesDtoModel
    {
        [Required(ErrorMessage = "selection value is required.")]
        public string t4_1_selection_values_name { get; set; }
        public string id_t4_selection { get; set; }
    }
    public class UpdateSelectionValuesDtoModel : SelectionValuesDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t4_1_selection_values { get; set; }

    }

    public class SysSelectionValuesDtoModel
    {

        public string t4_1_sys_selection_values_name { get; set; }
        public string id_t4_selection { get; set; }
    }

    public class UpdateSysSelectionValuesDtoModel : SysSelectionValuesDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t4_sys_selection_values { get; set; }

    }

    public class UserSelectionValuesDtoModel
    {

        public string t4_1_user_selection_values_name { get; set; }
        public string id_t4_sys_selection_values { get; set; }
        public string id_t4_selection { get; set; }
    }

    public class UpdateUserSelectionValuesDtoModel : UserSelectionValuesDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t4_user_selection_values { get; set; }

    }
}