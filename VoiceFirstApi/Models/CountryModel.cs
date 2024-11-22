using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.Models
{
    public class CountryModel
    {
        public string id_t2_1_country { get; set; }
        public string t2_1_country_name { get; set; }
        public string t2_1_div1_called { get; set; }
        public string t2_1_div2_called { get; set; }
        public string t2_1_div3_called { get; set; }
        public string? inserted_by { get; set; }
        public DateTime? inserted_date { get; set; }
        public string? updated_by { get; set; }
        public DateTime? updated_date { get; set; }
    }
}