using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class InsertCompanyDTOModel
    {
        [Required(ErrorMessage = "Company name is required.")]
        public string t1_company_name { get; set; }
        [Required(ErrorMessage = "Company Type is required.")]
        public string id_company_type { get; set; }
        public string? id_currency { get; set; }
        public DateTime? is_active_till_date { get; set; }
        [Required(ErrorMessage = "Branch is required.")]
        public InsertBranchDTOModel insertBranchDTOModel { get; set; } = new InsertBranchDTOModel();
        [Required(ErrorMessage = "User is required.")]
        public UserDtoModel userDtoModel { get; set; } = new UserDtoModel();
    }
}
