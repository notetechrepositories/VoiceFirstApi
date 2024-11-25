using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class DivisionOneDtoModel
    {

        [Required(ErrorMessage = "Division one name is required.")]
        public string t2_1_div1_name { get; set; }
        
        [Required(ErrorMessage = " Country Idis required.")]
        public string id_t2_1_country { get; set; }
    }
    public class UpdateDivisionOneDtoModel : DivisionOneDtoModel
    {
        [Required(ErrorMessage = "Division one id is required")]
        public string id_t2_1_div1 { get; set; }
    }
}