using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;

namespace VoiceFirstApi.Controllers
{
    [Route("api/sub-section")]
    [ApiController]
    public class SubSectionController : ControllerBase
    {
        private readonly ISubSectionService _SubSectionService;

        public SubSectionController(ISubSectionService SubSectionService)
        {
            _SubSectionService = SubSectionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] InsertSubSectionDTOModel insert)
        {
            try
            {
                var (data, message, status) = await _SubSectionService.AddAsync(insert);
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
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSubSectionDTOModel update)
        {
            try
            {
                var (data, message, status) = await _SubSectionService.UpdateAsync(update);
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
                var (data, message, status_code) = await _SubSectionService.GetAllAsync(filters.filters);
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
                var (data, message, status) = await _SubSectionService.GetByIdAsync(filters.id, filters.filters);
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
                var (data, message, status) = await _SubSectionService.DeleteAsync(id);
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
                var (data, message, status_code) = await _SubSectionService.UpdateStatusAsync(updateStatusDtoModel);
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
