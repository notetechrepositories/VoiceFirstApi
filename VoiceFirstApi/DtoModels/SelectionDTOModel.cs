using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class SelectionDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string t4_selection_name { get; set; }
    }
    public class UpdateSelectionDtoModel : SelectionDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t4_selection { get; set; }


    }
}