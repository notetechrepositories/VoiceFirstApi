namespace VoiceFirstApi.Models
{
    public class BranchModel:LocalModel
    {
        public string id_t2_company_branch { get; set; }
        public string id_t1_company { get; set; }
        public string t2_company_branch_name { get; set; }
        public string t2_id_branch_type { get; set; }
        public string company_branch_type_name { get; set; }
        public string t2_address_1 { get; set; }
        public string? t2_address_2 { get; set; }
        public string t2_zip_code { get; set; }
        public string? t2_mobile_no { get; set; }
        public string? t2_phone_no { get; set; }
        public string t2_email { get; set; }
        public int is_delete { get; set; }
        public int is_active { get; set; }

        public string id_t2_1_local { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
    public class BranchWithCompanyModel : LocalModel
    {
        public string id_t2_company_branch { get; set; }
        public string id_t1_company { get; set; }
        public string t2_company_branch_name { get; set; }
        public string t2_id_branch_type { get; set; }
        public string company_branch_type_name { get; set; }
        public string t2_address_1 { get; set; }
        public string? t2_address_2 { get; set; }
        public string t2_zip_code { get; set; }
        public string? t2_mobile_no { get; set; }
        public string? t2_phone_no { get; set; }
        public string t2_email { get; set; }
        public CompanyModel company_details { get; set; } = new CompanyModel();
    }
}