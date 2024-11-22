using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class DivisionOneDtoModel
    {

        [Required(ErrorMessage = "Name is required.")]
        public string t2_1_div1_name { get; set; }
        
        [Required(ErrorMessage = " Country is required.")]
        public string id_t2_1_country { get; set; }
    }
    public class UpdateDivisionOneDtoModel : DivisionOneDtoModel
    {
        [Required(ErrorMessage = "Country ID is required")]
        public string id_t2_1_div1 { get; set; }
    }
}