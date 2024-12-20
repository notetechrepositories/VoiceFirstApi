/*using Dapper;
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
    [Route("api/Local")]
    [ApiController]
    public class LocalController : ControllerBase
    {
        private readonly ILocalService _LocalService;

        public LocalController(ILocalService LocalService)
        {
            _LocalService = LocalService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] LocalDtoModel LocalDto)
        {
            try
            {
                var (data, message, status_code) = await _LocalService.AddAsync(LocalDto);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateLocalDtoModel LocalDto)
        {
            try
            {
                var (data, message, status_code) = await _LocalService.UpdateAsync(LocalDto);
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

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                var (data, message, status_code) = await _LocalService.GetAllAsync(filters);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            try
            {
                var (data, message, status_code) = await _LocalService.GetByIdAsync(id, filters);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {

                var (data, message, status_code) = await _LocalService.DeleteAsync(id);
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
}*/