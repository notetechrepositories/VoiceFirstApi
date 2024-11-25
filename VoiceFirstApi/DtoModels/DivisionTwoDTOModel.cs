using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class DivisionTwoDtoModel
    {
        [Required(ErrorMessage = "Division two Name is required.")]
        public string t2_1_div2_name { get; set; }
        [Required(ErrorMessage = "Division one id is required.")]
        public string id_t2_1_div1 { get; set; }
    }
    public class UpdateDivisionTwoDtoModel : DivisionTwoDtoModel
    {
        [Required(ErrorMessage = "Division two id is required.")]
        public string id_t2_1_div2 { get; set; }

    }
}