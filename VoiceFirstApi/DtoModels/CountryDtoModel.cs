using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class CountryDtoModel
    {
        [Required(ErrorMessage = "Country name is required.")]
        public string t2_1_country_name { get; set; }

        public string? t2_1_div1_called { get; set; }
        public string? t2_1_div2_called { get; set; }
        public string? t2_1_div3_called { get; set; }
    }

    public class UpdateCountryDtoModel : CountryDtoModel
    {
        [Required(ErrorMessage = "Country ID is required")]
        public string id_t2_1_country { get; set; }
    }
}