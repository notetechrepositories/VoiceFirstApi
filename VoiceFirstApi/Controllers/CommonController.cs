using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Service;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _CommonService;
        private const string StoredHash = "0Idek6onSriWB0jvkK9/5LxenvQBJy6aeseHPgbJ568=";
        private const string StoredSalt = "3Zw62gXwNbRzCDX2RZCMRw==";

        public CommonController(ICommonService CommonService)
        {
            _CommonService = CommonService;
        }

        

        // POST: api/Auth/VerifyPassword
        [HttpPost("VerifyPassword")]
        public IActionResult VerifyPassword(int Password)
        {
            if (Password <= 0)
            {
                return BadRequest(new { Message = "Input must be greater than 0." });
            }

            int maxNumber = (int)Math.Pow(10, Password) - 1; // Calculate the max value for the loop
            var results = new List<int>();

            for (int i = 1; i <= maxNumber; i++)
            {
                bool isPasswordValid = SecurityUtilities.VerifyPassword(i.ToString(), StoredHash, StoredSalt);

                if (isPasswordValid)
                {
                    return Ok(new { Message = "Password is valid!",data=i });
                }
            }
            return Ok(new { Message = "Password is invalid!" });
            // Check if the user input matches the stored password hash and salt

        }

        [Authorize]
        [HttpPost("import-divisions")]
        public async Task<IActionResult> ImportDivisions(List<ImportDivisionThreeModel> model)
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
