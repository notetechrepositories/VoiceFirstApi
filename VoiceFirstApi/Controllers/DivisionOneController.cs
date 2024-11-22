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
    [Route("api/DivisionOne")]
    [ApiController]
    public class DivisionOneController : ControllerBase
    {
        private readonly IDivisionOneService _DivisionOneService;

        public DivisionOneController(IDivisionOneService DivisionOneService)
        {
            _DivisionOneService = DivisionOneService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] DivisionOneDtoModel DivisionOneDto)
        {
            var (data, status) = await _DivisionOneService.AddAsync(DivisionOneDto);
            return Ok(new { data = data, message = status });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] DivisionOneDtoModel DivisionOneDto)
        {
            var (data, status) = await _DivisionOneService.UpdateAsync(DivisionOneDto);
            return Ok(new { data = data, message = status });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, object> filters)
        {
            var (data, status) = await _DivisionOneService.GetAllAsync(filters);
            return Ok(new { data = data, message = status });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, object> filters)
        {
            var (data, status) = await _DivisionOneService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, status) = await _DivisionOneService.DeleteAsync(id);
            return Ok(new { data = data, message = status });
        }
    }
}