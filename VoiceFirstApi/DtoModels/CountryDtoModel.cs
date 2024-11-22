using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class CountryDtoModel
    {

        public string id_t2_1_country { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        [StringLength(100, ErrorMessage = "Country name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string t2_1_country_name { get; set; }
        
        [StringLength(100, ErrorMessage = "Division 1 name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string t2_1_div1_called { get; set; }
        [StringLength(100, ErrorMessage = "Division 2 name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string t2_1_div2_called { get; set; }
        [StringLength(100, ErrorMessage = "Division 3 name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string t2_1_div3_called { get; set; }
    }
}
