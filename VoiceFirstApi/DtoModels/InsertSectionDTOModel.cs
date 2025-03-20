namespace VoiceFirstApi.DtoModels
{
    public class InsertSectionDTOModel
    {
        public string id_t2_company_branch { get; set; }
        public string section_name { get; set; }
    }
    public class UpdateSectionDTOModel : InsertSectionDTOModel
    {
        public string id_t3_branch_section { get; set; }
    }
}
