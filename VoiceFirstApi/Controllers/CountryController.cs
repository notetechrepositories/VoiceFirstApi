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
    [Route("api/Country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _CountryService;

        public CountryController(ICountryService CountryService)
        {
            _CountryService = CountryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CountryDtoModel CountryDto)
        {
            var (data, status) = await _CountryService.AddAsync(CountryDto);
            return Ok(new { data = data, message = status });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCountryDtoModel CountryDto)
        {
            var (data, status) = await _CountryService.UpdateAsync(CountryDto);
            return Ok(new { data = data, message = status });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, object> filters)
        {
            var (data, status) = await _CountryService.GetAllAsync(filters);
            return Ok(new { data = data, message = status });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, object> filters)
        {
            var (data, status) = await _CountryService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, status) = await _CountryService.DeleteAsync(id);
            return Ok(new { data = data, message = status });
        }
    }
}