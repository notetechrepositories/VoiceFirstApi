namespace VoiceFirstApi.DtoModels
{
    public class InsertSubSectionDTOModel
    {
        public string id_t3_branch_section { get; set; }
        public string sub_section_name { get; set; }
    }
    public class UpdateSubSectionDTOModel : InsertSubSectionDTOModel
    {
        public string id_t3_branch_sub_section { get; set; }
    }
}
