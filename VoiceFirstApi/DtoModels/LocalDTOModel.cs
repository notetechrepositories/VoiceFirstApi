using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class LocalDtoModel
    {

        public string? id_t2_1_country { get; set; }
        public string? id_t2_1_div1 { get; set; }
        public string? id_t2_1_div2 { get; set; }
        public string? id_t2_1_div3 { get; set; }
        public string? t2_1_local_name { get; set; }
    }
    public class UpdateLocalDtoModel : LocalDtoModel
    {
        [Required(ErrorMessage = "Local id is required.")]
        public string id_t2_1_local { get; set; }

    }
}
