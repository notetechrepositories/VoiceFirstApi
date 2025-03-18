using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Service;
using VoiceFirstApi.Utilities;

namespace VoiceFirstApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpPost("user-register")]
        public async Task<IActionResult> AddAsync([FromBody] UserDtoModel UserDto)
        {
            try
            {
                UserDto.id_t5_1_m_user_roles = "FC970C84-E654-4C7F-9893-87D1D2EF03F5";
                var (data, message, status_code) = await _UserService.AddAsync(UserDto);
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
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDtoModel UserDto)
        {
            try
            {

                var (data, message, status_code) = await _UserService.AddAsync(UserDto);
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
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserDtoModel UserDto)
        {
            try
            {
                var (data, message, status_code) = await _UserService.UpdateAsync(UserDto);
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

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _UserService.GetAllAsync(filters.filters);
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

        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetByIdAsync([FromBody] FiltersAndIdDtoModel filters)
        {
            try
            {
                // Validate user identity
                
               
                var (data, message, status_code) = await _UserService.GetByIdAsync(filters.id, filters.filters);
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

        [HttpGet("get-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userClaims = HttpContext?.User;
                var empty = new Dictionary<string, object>();
                if (userClaims == null || !userClaims.Identity.IsAuthenticated)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }

                // Find the user_id claim
                var userIdClaim = userClaims.FindFirst("user_id");
                if (userIdClaim == null)
                {
                    throw new UnauthorizedAccessException("User ID not found in the token.");
                }
                var decryUserId = SecurityUtilities.Decryption(userIdClaim.Value);
                if (decryUserId == null)
                {
                    throw new UnauthorizedAccessException("User ID not found in the token.");
                }
                Dictionary<string, string>? filters= new Dictionary<string, string>();
                var (data, message, status_code) = await _UserService.GetByIdAsync(decryUserId, filters);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _UserService.DeleteAsync(id);
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
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            try
            {
                var (data, message, status_code) = await _UserService.UpdateStatus(updateStatusDtoModel);
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