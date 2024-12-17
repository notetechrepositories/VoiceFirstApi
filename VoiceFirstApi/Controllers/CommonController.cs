using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Service;

namespace VoiceFirstApi.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _CommonService;

        public CommonController(ICommonService CommonService)
        {
            _CommonService = CommonService;
        }

        [HttpPost("import-divisions")]
        public async Task<IActionResult> importDivisions(List<ImportDivisionThreeModel> model)
        {
            try
            {
                var (data, message, status_code) = await _CommonService.importDivisions(model);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, new
                {
                    data = (object)null,
                    message = "An error occurred while processing your request.",
                    status = 500,
                    error = ex.Message
                });
            }


        }
    }
}
