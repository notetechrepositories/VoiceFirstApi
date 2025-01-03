namespace VoiceFirstApi.Models
{
    public class GetAllProgramWithActions
    {
        public string t6_program_name { get; set; }
        public List<ProgramActions> programActions { get; set; }

    }
    public class ProgramActions
    {
        public string id_t6_link_program_with_program_action { get; set; }
        public string t6_action { get; set;}

    }
     public class ProgramModel
    {
        public string id_t6_program { get; set; }
        public string t6_program_name { get; set; }
    }
    public class GetAllActionWithProgramId 
    {
        public string id_t6_link_program_with_program_action { get; set; }
        public string id_t6_program { get; set; }
        public string id_t6_program_action { get; set; }
        public string t6_action { get; set; }
    }
     
}
