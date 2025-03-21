using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using VoiceFirstApi.Context;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Service;

namespace VoiceFirstApi.Controllers
{
    [Route("api/selection_values")]
    [ApiController]
    public class SelectionValuesController : ControllerBase
    {
        private readonly ISelectionValuesService _SelectionValuesService;

        public SelectionValuesController(ISelectionValuesService SelectionValuesService)
        {
            _SelectionValuesService = SelectionValuesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] SelectionValuesDtoModel SelectionValuesDto)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.AddAsync(SelectionValuesDto);
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

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSelectionValuesDtoModel SelectionValuesDto)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.UpdateAsync(SelectionValuesDto);
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

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.GetAllAsync(filters.filters);
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
        [HttpGet("get-all-branch-type")]
        [Authorize]
        public async Task<IActionResult> GetBranchType()
        {
            try
            {

                var (data, message, status_code) = await _SelectionValuesService.GetBranchType();
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
        [HttpGet("get-all-company-type")]
        [Authorize]
        public async Task<IActionResult> GetCompanyType()
        {
            try
            {
                
                var (data, message, status_code) = await _SelectionValuesService.GetCompanyType();
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

        [HttpGet("get-all-role-type")]
        public async Task<IActionResult> GetRoleType()
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.GetRoleType();
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
                var (data, message, status_code) = await _SelectionValuesService.GetByIdAsync(filters.id, filters.filters);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.DeleteAsync(id);
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
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.UpdateStatus(updateStatusDtoModel);
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


        //---------------------------------------------------------------- Sys selection values --------------------------------------------------------------------------------


        [HttpPost("add-sys-selection-values")]
        public async Task<IActionResult> AddSysAsync([FromBody] SysSelectionValuesDtoModel SelectionValuesDto)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.AddSysAsync(SelectionValuesDto);
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

        [HttpPut("update-sys-selection-values")]
        public async Task<IActionResult> UpdateSysAsync([FromBody] UpdateSysSelectionValuesDtoModel SelectionValuesDto)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.UpdateSysAsync(SelectionValuesDto);
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

        [HttpPost("get-all-sys-selection-values")]
        public async Task<IActionResult> GetAllSysAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.GetAllSysAsync(filters.filters);
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
        [HttpPost("all-sys-selection-values-by-selection-id")]
        public async Task<IActionResult> GetAllSysSelectionValueBySelectionId([FromBody] string selectionId)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.GetAllSysValuesBySectionTypeAsync(selectionId);
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

        [HttpDelete("sys-selection-values-delete")]
        public async Task<IActionResult> DeleteSysAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.DeleteSysAsync(id);
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
        [HttpPut("update-sys-selection-values-status")]
        public async Task<IActionResult> UpdateSysStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.UpdateSysStatus(updateStatusDtoModel);
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

        //------------------------------------------------------------------ user selection values ------------------------------------------------------------------------------


        [HttpPost("add-user-selection-values")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserSelectionValuesDtoModel SelectionValuesDto)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.AddUserAsync(SelectionValuesDto);
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

        [HttpPut("update-user-selection-values")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserSelectionValuesDtoModel SelectionValuesDto)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.UpdateUserAsync(SelectionValuesDto);
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

        [HttpPost("get-all-user-selection-values")]
        public async Task<IActionResult> GetAllUserAsync([FromBody] FilterDtoModel filters)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.GetAllUserAsync(filters.filters);
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

        [HttpPost("all-user-selection-values-by-selection-id")]
        public async Task<IActionResult> GetAllSelectionValueBySelectionId([FromBody] string selectionId)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.GetAllValuesBySectionTypeAsync(selectionId);
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
        [HttpDelete("user-selection-values-delete")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.DeleteUserAsync(id);
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
        [HttpPut("update-user-selection-values-status")]
        public async Task<IActionResult> UpdateUserStatus(UpdateStatusDtoModel updateStatusDtoModel)
        {
            try
            {
                var (data, message, status_code) = await _SelectionValuesService.UpdateUserStatus(updateStatusDtoModel);
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