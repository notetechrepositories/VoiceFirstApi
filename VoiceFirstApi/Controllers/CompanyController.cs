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
    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _CompanyService;

        public CompanyController(ICompanyService CompanyService)
        {
            _CompanyService = CompanyService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CompanyDtoModel CompanyDto)
        {
            var (data, message, status_code) = await _CompanyService.AddAsync(CompanyDto);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCompanyDtoModel CompanyDto)
        {
            var (data, message, status_code) = await _CompanyService.UpdateAsync(CompanyDto);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            var (data, message, status_code) = await _CompanyService.GetAllAsync(filters);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            var (data, message, status_code) = await _CompanyService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, message, status_code) = await _CompanyService.DeleteAsync(id);
            return Ok(new { data = data, message = message, status = status_code });
        }
    }
}