﻿namespace VoiceFirstApi.Models
{
    public class RoleModel
    {
        public string id_t5_1_m_user_roles { get; set; }
        public string t5_1_m_user_roles_name { get; set; }
        public int t5_1_m_all_location_access { get; set; }
        public int t5_1_m_all_location_type { get; set; }
        public int t5_1_m_only_assigned_location { get; set; }
        public string id_t4_1_selection_values { get; set; }
        public string t5_1_m_type_id { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }

    public class SysRoleModel
    {
        public string id_t5_1_sys_roles { get; set; }
        public string t5_1_sys_roles_name { get; set; }
        public string t5_1_sys_all_location_access { get; set; }
        public string t5_1_sys_all_issues { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
    public class CompanyRoleModel
    {
        public string id_t5_1_company_roles { get; set; }
        public string id_t1_company { get; set; }
        public string id_t5_1_sys_roles { get; set; }
        public string t5_1_roles_name { get; set; }
        public string t5_1_all_location_access { get; set; }
        public string t5_1_all_issues { get; set; }
        public string inserted_by { get; set; }
        public DateTime inserted_date { get; set; }
        public string updated_by { get; set; }
        public DateTime updated_date { get; set; }
    }
}