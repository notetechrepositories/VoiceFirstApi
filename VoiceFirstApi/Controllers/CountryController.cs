using CsvHelper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.NetworkInformation;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
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
            var (data, message, status_code) = await _CountryService.AddAsync(CountryDto);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCountryDtoModel CountryDto)
        {
            var (data, message, status_code) = await _CountryService.UpdateAsync(CountryDto);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            var (data, message, status_code) = await _CountryService.GetAllAsync(filters);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
        {
            var (data, message, status_code) = await _CountryService.GetByIdAsync(id, filters);
            return Ok(new { data = data, message = message, status = status_code });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var (data, message, status_code) = await _CountryService.DeleteAsync(id);
            return Ok(new { data = data, message = message, status = status_code });
        }

        /*[HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<ImportCountryModel>().ToList();
                var (data, message, status_code) = await _CountryService.ImportCountry(records);
                return Ok(new { data = data, message = message,status= status_code });
            }
        }*/
        [HttpPost("import")]
        public async Task<IActionResult> UploadXl(List<ImportCountryModel> model)
        {
            var (data, message, status_code) = await _CountryService.ImportCountry(model);
            return Ok(new { data = data, message = message, status = status_code });
           
        }
    }
}