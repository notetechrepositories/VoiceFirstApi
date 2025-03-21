﻿using CsvHelper;
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

namespace VoiceFirstApi.Controllers
{
    [Route("api/division-three")]
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
            try
            {
                var (data, message, status_code) = await _DivisionThreeService.AddAsync(DivisionThreeDto);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
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
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDivisionThreeDtoModel DivisionThreeDto)
        {
            try
            {
                var (data, message, status_code) = await _DivisionThreeService.UpdateAsync(DivisionThreeDto);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, new
                {
                    data = (object)null,
                    message = "An error occurred while processing your request.",
                    status = 500,
                    error = ex.Message
                });
            }
       
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _DivisionThreeService.GetAllAsync(filters.filters);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, new
                {
                    data = (object)null,
                    message = "An error occurred while processing your request.",
                    status = 500,
                    error = ex.Message
                });
            }
          
        }

        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetByIdAsync([FromBody] FiltersAndIdDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _DivisionThreeService.GetByIdAsync(filters.id, filters.filters);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
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
                var (data, message, status_code) = await _DivisionThreeService.DeleteAsync(id);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, new
                {
                    data = (object)null,
                    message = "An error occurred while processing your request.",
                    status = 500,
                    error = ex.Message
                });
            }
            
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
                 var records = csv.GetRecords<ImportDivisionThreeModel>().ToList();
                 var (data, message, status_code) = await _DivisionThreeService.ImportDivisionThree(records);
                 return Ok(new { data = data, message = message, status = status_code });
             }
         }*/
        [HttpPost("import")]
        public async Task<IActionResult> UploadXl(List<ImportDivisionThreeModel> model)
        {
            try
            {
                var (data, message, status_code) = await _DivisionThreeService.ImportDivisionThree(model);
                return Ok(new { data = data, message = message, status = status_code });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, new
                {
                    data = (object)null,
                    message = "An error occurred while processing your request.",
                    status = 500,
                    error = ex.Message
                });
            }
            

        }

        /*[HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            try
            {
                var (data, message, status_code) = await _DivisionThreeService.UpdateStatus(updateStatusDtoModel);
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
        }*/
    }
}