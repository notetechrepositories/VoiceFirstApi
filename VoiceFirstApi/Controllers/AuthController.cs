using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Service;

namespace VoiceFirstApi.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _AuthService;

        public AuthController(IAuthService AuthService)
        {
            _AuthService = AuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthLogin([FromBody] AuthDtoModel AuthDto)
        {
            try
            {
                var (data, message, status_code) = await _AuthService.AuthLogin(AuthDto);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
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
