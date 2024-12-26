using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VoiceFirstApi.Controllers
{
    [Route("api/UserCompanyLink")]
    [ApiController]
    public class UserCompanyLinkController : ControllerBase
    {
        private readonly IUserCompanyLinkService _UserCompanyLinkService;

        public UserCompanyLinkController(IUserCompanyLinkService UserCompanyLinkService)
        {
            _UserCompanyLinkService = UserCompanyLinkService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserCompanyLinkDtoModel UserCompanyLinkDto)
        {
            try
            {
                var (data, message, status_code) = await _UserCompanyLinkService.AddAsync(UserCompanyLinkDto);
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
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserCompanyLinkDtoModel UserCompanyLinkDto)
        {
            try
            {
                var (data, message, status_code) = await _UserCompanyLinkService.UpdateAsync(UserCompanyLinkDto);
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                var (data, message, status_code) = await _UserCompanyLinkService.GetAllAsync(filters);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                var (data, message, status_code) = await _UserCompanyLinkService.GetByIdAsync(id, filters);
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
                var (data, message, status_code) = await _UserCompanyLinkService.DeleteAsync(id);
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