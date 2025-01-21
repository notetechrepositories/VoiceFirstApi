namespace VoiceFirstApi.Models
{
    public class CompanyWithBranchAndUserDetailsModel: CompanyModel
    {
        public List<BranchModel> branchDetailsModel { get; set; } = new List<BranchModel>();
        
    }
    public class BranchDetailsModel:LocalModel 
    {
        public string id_t2_company_branch { get; set; }
        public string id_t1_company { get; set; }
        public string t2_company_branch_name { get; set; }
        public string t2_id_branch_type { get; set; }
        public string t2_address_1 { get; set; }
        public string? t2_address_2 { get; set; }
        public string t2_zip_code { get; set; }
        public string? t2_mobile_no { get; set; }
        public string? t2_phone_no { get; set; }
        public string t2_email { get; set; }
        public long user_count { get; set; }
       
    }
}
