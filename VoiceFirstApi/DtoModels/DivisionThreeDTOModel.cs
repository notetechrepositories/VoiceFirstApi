using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class DivisionThreeDtoModel
    {
        [Required(ErrorMessage = "division three is required.")]
        public string t2_1_div3_name { get; set; }

        [Required(ErrorMessage = "division two id is required.")]
        public string id_t2_1_div2 { get; set; }
    }
    public class UpdateDivisionThreeDtoModel : DivisionThreeDtoModel
    {
        [Required(ErrorMessage = "division three id is required.")]
        public string id_t2_1_div3 { get; set; }

    }
}