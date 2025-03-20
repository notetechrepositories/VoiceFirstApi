using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Service;

namespace VoiceFirstApi.Controllers
{
    [Route("api/section")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _SectionService;

        public SectionController(ISectionService SectionService)
        {
            _SectionService = SectionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] InsertSectionDTOModel insert)
        {
            try
            {
                var (data, message, status) = await _SectionService.AddAsync(insert);
                return Ok(new { data = data, message = message, status = status });
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
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSectionDTOModel update)
        {
            try
            {
                var (data, message, status) = await _SectionService.UpdateAsync(update);
                return Ok(new { data = data, message = message, status = status });
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
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _SectionService.GetAllAsync(filters.filters);
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
        }
        [HttpPost("get-all-section")]
        public async Task<IActionResult> GetAllSectionAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _SectionService.GetAllSectionAsync(filters.filters);
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
        }
        [HttpPost("get-by-id")]
        public async Task<IActionResult> GetByIdAsync([FromBody] FiltersAndIdDtoModel filters)
        {
            try
            {
                var (data, message, status) = await _SectionService.GetByIdAsync(filters.id, filters.filters);
                return Ok(new { data = data, message = message, status = status });
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
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var (data, message, status) = await _SectionService.DeleteAsync(id);
                return Ok(new { data = data, message = message, status = status });
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
        }
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            try
            {
                var (data, message, status_code) = await _SectionService.UpdateStatusAsync(updateStatusDtoModel);
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
        }
    }
}
