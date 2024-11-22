using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class TestDtoModel
    {
        public string id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string name { get; set; }
    }
}