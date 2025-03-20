namespace VoiceFirstApi.Models
{
    public class SectionModel
    {
        public string id_t3_branch_section { get; set; }
        public string id_t2_company_branch { get; set; }
        public string t4_selection_name { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
    public class SectionModelWithBranch
    {
        public string id_t3_branch_section { get; set; }
        public string id_t2_company_branch { get; set; }
        public string t4_selection_name { get; set; }

        public BranchModel branch_details { get; set; } = new BranchModel();
        public CompanyModel company_details { get; set; } = new CompanyModel();
    }
}
