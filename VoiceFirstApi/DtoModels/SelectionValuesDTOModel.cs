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
}