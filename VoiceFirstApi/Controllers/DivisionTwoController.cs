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
    [Route("api/DivisionTwo")]
    [ApiController]
    public class DivisionTwoController : ControllerBase
    {
        private readonly IDivisionTwoService _DivisionTwoService;

        public DivisionTwoController(IDivisionTwoService DivisionTwoService)
        {
            _DivisionTwoService = DivisionTwoService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] DivisionTwoDtoModel DivisionTwoDto)
        {
            var (data, status) = await _DivisionTwoService.AddAsync(DivisionTwoDto);
            return Ok(new { data = data, message = status });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDivisionTwoDtoModel DivisionTwoDto)
        {
            var (data, status) = await _DivisionTwoService.UpdateAsync(DivisionTwoDto);
            return Ok(new { data = data, message = status });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            var (data, status) = await _DivisionTwoService.GetAllAsync(filters);
            return Ok(new { data = data, message = status });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            var (data, status) = await _DivisionTwoService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, status) = await _DivisionTwoService.DeleteAsync(id);
            return Ok(new { data = data, message = status });
        }
    }
}