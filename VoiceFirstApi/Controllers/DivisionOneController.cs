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
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDivisionOneDtoModel DivisionOneDto)
        {
            var (data, status) = await _DivisionOneService.UpdateAsync(DivisionOneDto);
            return Ok(new { data = data, message = status });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] Dictionary<string, string> filters)
        {
            var (data, status) = await _DivisionOneService.GetAllAsync(filters);
            return Ok(new { data = data, message = status });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromQuery] Dictionary<string, string> filters)
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

        [HttpPost("upload")]
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
                var records = csv.GetRecords<ImportDivisionOneModel>().ToList();
                var (data, status) = await _DivisionOneService.ImportStateByCountry(records);
                return Ok(new { data = data, message = status });
            }
        }
    }
}