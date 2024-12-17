using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class CompanyDtoModel
    {
        [Required(ErrorMessage = "Company name is required.")]
        public string t1_company_name { get; set; }
        public string id_company_type { get; set; }
        public string id_currency { get; set; }
        public DateTime is_active_till_date { get; set; }
       
    }
    public class UpdateCompanyDtoModel : CompanyDtoModel
    {
        [Required(ErrorMessage = "Company id is required.")]
        public string id_t1_company { get; set; }

    }
}