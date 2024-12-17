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
            var (data, message, status_code) = await _LocalService.AddAsync(LocalDto);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateLocalDtoModel LocalDto)
        {
            var (data, message, status_code) = await _LocalService.UpdateAsync(LocalDto);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            var (data, message, status_code) = await _LocalService.GetAllAsync(filters);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            var (data, message, status_code) = await _LocalService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, message, status_code) = await _LocalService.DeleteAsync(id);
            return Ok(new { data = data, message = message, status = status_code });
        }
    }
}