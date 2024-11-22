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
    [Route("api/Test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _TestService;

        public TestController(ITestService TestService)
        {
            _TestService = TestService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] TestDtoModel TestDto)
        {
            var (data, status) = await _TestService.AddAsync(TestDto);
            return Ok(new { data = data, message = status });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] TestDtoModel TestDto)
        {
            var (data, status) = await _TestService.UpdateAsync(TestDto);
            return Ok(new { data = data, message = status });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, object> filters)
        {
            var (data, status) = await _TestService.GetAllAsync(filters);
            return Ok(new { data = data, message = status });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, object> filters)
        {
            var (data, status) = await _TestService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, status) = await _TestService.DeleteAsync(id);
            return Ok(new { data = data, message = status });
        }
    }
}