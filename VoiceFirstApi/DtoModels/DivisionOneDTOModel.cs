using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class DivisionOneDtoModel
    {
        public string id_t2_1_div1 { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string t2_1_div1_name { get; set; }
        
        [Required(ErrorMessage = " Country is required.")]
        [StringLength(100, ErrorMessage = "Name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string id_t2_1_country { get; set; }
    }
}