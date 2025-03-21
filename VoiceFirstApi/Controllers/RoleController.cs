﻿using Dapper;
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
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _RoleService;

        public RoleController(IRoleService RoleService)
        {
            _RoleService = RoleService;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddAsync([FromBody] RoleDtoModel RoleDto)
        //{
        //    try
        //    {
        //        var (data, message, status_code) = await _RoleService.AddAsync(RoleDto);
        //        return Ok(new { data = data, message = message, status = status_code });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            data = (object)null,
        //            message = "An error occurred while processing your request.",
        //            status = 500,
        //            error = ex.Message
        //        });
        //    }
        //}
        [HttpPost("add-role-with-permission")]
        public async Task<IActionResult> AddRoleWithPermissionAsync([FromBody] InsertRoleWithPermissionDTOModel RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.AddRoleWithPermissionAsync(RoleDto);
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
        //[HttpPut]
        //public async Task<IActionResult> UpdateAsync([FromBody] UpdateRoleDtoModel RoleDto)
        //{
        //    try
        //    {
        //        var (data, message, status_code) = await _RoleService.UpdateAsync(RoleDto);
        //        return Ok(new { data = data, message = message, status = status_code });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            data = (object)null,
        //            message = "An error occurred while processing your request.",
        //            status = 500,
        //            error = ex.Message
        //        });
        //    }
        //}
        [HttpPut("update-role-with-permission")]
        public async Task<IActionResult> UpdateRoleWithPermissionAsync([FromBody] UpdateRoleWithPermissionDtoModel RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.UpdateRoleWithPermissionAsync(RoleDto);
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
        [HttpGet("get-permission-by-role-id")]
        public async Task<IActionResult> GetByRoleIdAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.GetByRoleIdAsync(id);
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

        [HttpGet("get-all")]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var (data, message, status_code) = await _RoleService.GetAllAsync();
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
                var (data, message, status_code) = await _RoleService.GetByIdAsync(filters.id, filters.filters);
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
                var (data, message, status_code) = await _RoleService.DeleteAsync(id);
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
                var (data, message, status_code) = await _RoleService.UpdateStatus(updateStatusDtoModel);
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
        [HttpGet("get-all-program-with-action")]
        public async Task<IActionResult> GetAllProgramWithActions()
        {
            try
            {
                var (data, message, status_code) = await _RoleService.GetAllProgramWithActions();
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



        //------------------------------------ NeW Sys Role-------------------------------------


        [HttpGet("get-all-sys-role")]
        [Authorize]
        public async Task<IActionResult> GetAllSysRoleAsync()
        {
            try
            {
                var (data, message, status_code) = await _RoleService.GetAllSysRoleAsync();
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
        [HttpDelete("delete-sys-role")]
        [Authorize]
        public async Task<IActionResult> DeleteSysRoleAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.DeleteSysRoleAsync(id);
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

        [HttpPost("add-sys-role")]
        [Authorize]
        public async Task<IActionResult> AddSysRoleAsync([FromBody] SysRoleDtoModel RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.AddSysRoleAsync(RoleDto);
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
        [HttpPut("update-sys-role")]
        [Authorize]
        public async Task<IActionResult> UpdateSysRoleAsync([FromBody] UpdateSysRoleDtoModel RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.UpdateSysRoleAsync(RoleDto);
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


        //[HttpPost("list-of-add-sys-role")]
        //[Authorize]
        //public async Task<IActionResult> ListOfAddSysRoleAsync([FromBody] List<SysRoleDtoModel> RoleDto)
        //{
        //    try
        //    {
        //        var (data, message, status_code) = await _RoleService.ListAddSysRoleAsync(RoleDto);
        //        return Ok(new { data = data, message = message, status = status_code });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            data = (object)null,
        //            message = "An error occurred while processing your request.",
        //            status = 500,
        //            error = ex.Message
        //        });
        //    }
        //}
        //------------------------------------ NeW Company Role-------------------------------------

        [HttpGet("get-all-company-role")]
        [Authorize]
        public async Task<IActionResult> GetAllCompanyRoleAsync()
        {
            try
            {
                var (data, message, status_code) = await _RoleService.GetAllCompanyRoleAsync();
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
        [HttpDelete("delete-company-role")]
        [Authorize]
        public async Task<IActionResult> DeleteCompanyRoleAsync(string id)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.DeleteCompanyRoleAsync(id);
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
        [HttpPost("add-company-role")]
        [Authorize]
        public async Task<IActionResult> AddCompanyRoleAsync([FromBody] CompanyRoleDtoModel RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.AddCompanyRoleAsync(RoleDto);
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

        [HttpPut("update-company-role")]
        public async Task<IActionResult> UpdateCompanyRoleAsync([FromBody] UpdateCompanyRoleDtoModel RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.UpdateCompanyRoleAsync(RoleDto);
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
        [HttpPost("list-of-add-company-role")]
        [Authorize]
        public async Task<IActionResult> ListOfAddCompanyRoleAsync([FromBody] List<CompanyRoleDtoModel> RoleDto)
        {
            try
            {
                var (data, message, status_code) = await _RoleService.ListOfAddCompanyRoleAsync(RoleDto);
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