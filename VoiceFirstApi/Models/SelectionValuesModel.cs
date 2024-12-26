namespace VoiceFirstApi.Models
{
    public class SelectionValuesModel
    {
        public string id_t4_1_selection_values { get; set; }
        public string id_t4_selection { get; set; }
        public string t4_1_selection_values_name { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
    public class GetRoleTypeModel: SelectionValuesModel
    {
        public List<RoleTypeModel> role_type_data { get; set; }

    }

    public class RoleTypeModel 
    {
        public string type_id { get; set; }
        public string type_name { get; set; }
    }
}