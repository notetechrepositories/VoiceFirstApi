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
    [Route("api/Selection")]
    [ApiController]
    public class SelectionController : ControllerBase
    {
        private readonly ISelectionService _SelectionService;

        public SelectionController(ISelectionService SelectionService)
        {
            _SelectionService = SelectionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SelectionDtoModel SelectionDto)
        {
            try
            {
                var (data,message,status) = await _SelectionService.AddAsync(SelectionDto);
                return Ok(new { data = data, message = message, status = status });
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
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSelectionDtoModel SelectionDto)
        {
            try
            {
                var (data, message, status) = await _SelectionService.UpdateAsync(SelectionDto);
                return Ok(new { data = data, message = message, status = status });
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
                var (data, message, status_code) = await _SelectionService.GetAllAsync(filters);
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
                var (data, message, status) = await _SelectionService.GetByIdAsync(id, filters);
                return Ok(new { data = data, message = message, status = status });
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
                var (data, message, status) = await _SelectionService.DeleteAsync(id);
                return Ok(new { data = data, message = message, status = status });
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