using System.ComponentModel.DataAnnotations;

namespace VoiceFirstApi.DtoModels
{
    public class InserPermissionDTOModel
    {

        [Required(ErrorMessage = "Name is required.")]
        public string permission { get; set; }
    }
}
