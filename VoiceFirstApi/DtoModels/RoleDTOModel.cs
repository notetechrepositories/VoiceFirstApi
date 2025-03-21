using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class RoleDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string t5_1_m_user_roles_name { get; set; }
        public int t5_1_m_all_location_access { get; set; }
        public int t5_1_m_all_location_type { get; set; }
        public int t5_1_m_only_assigned_location { get; set; }
        public string t5_1_m_type_id { get; set; }
        public string id_t4_1_selection_values { get; set; }
    }
    public class UpdateRoleDtoModel : RoleDtoModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string id_t5_1_m_user_roles { get; set; }

    }

    //------------------------------------ NeW Sys Role-------------------------------------

    public class SysRoleDtoModel
    {
        
        public string t5_1_sys_roles_name { get; set; }
        public string t5_1_sys_all_location_access { get; set; }
        public string t5_1_sys_all_issues { get; set; }
    }
    public class UpdateSysRoleDtoModel : SysRoleDtoModel
    {

        public string id_t5_1_sys_roles { get; set; }

    }


    //------------------------------------ NeW Company Role-------------------------------------

    public class CompanyRoleDtoModel
    {

        public string id_t5_1_sys_roles { get; set; }
        public string t5_1_roles_name { get; set; }
        public string t5_1_all_location_access { get; set; }
        public string t5_1_all_issues { get; set; }
    }
    public class UpdateCompanyRoleDtoModel : CompanyRoleDtoModel
    {
        public string id_t5_1_company_roles { get; set; }

    }

}