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
    [Route("api/DivisionThree")]
    [ApiController]
    public class DivisionThreeController : ControllerBase
    {
        private readonly IDivisionThreeService _DivisionThreeService;

        public DivisionThreeController(IDivisionThreeService DivisionThreeService)
        {
            _DivisionThreeService = DivisionThreeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] DivisionThreeDtoModel DivisionThreeDto)
        {
            var (data, status) = await _DivisionThreeService.AddAsync(DivisionThreeDto);
            return Ok(new { data = data, message = status });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDivisionThreeDtoModel DivisionThreeDto)
        {
            var (data, status) = await _DivisionThreeService.UpdateAsync(DivisionThreeDto);
            return Ok(new { data = data, message = status });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            var (data, status) = await _DivisionThreeService.GetAllAsync(filters);
            return Ok(new { data = data, message = status });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            var (data, status) = await _DivisionThreeService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, status) = await _DivisionThreeService.DeleteAsync(id);
            return Ok(new { data = data, message = status });
        }
    }
}