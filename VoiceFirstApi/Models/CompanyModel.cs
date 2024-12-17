namespace VoiceFirstApi.Models
{
    public class CompanyModel
    {
        public string id_t1_company { get; set; }
        public string t1_company_name { get; set; }
        public string id_company_type { get; set; }
        public string id_currency { get; set; }
        public string is_delete { get; set; }
        public DateTime is_active_till_date { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
}