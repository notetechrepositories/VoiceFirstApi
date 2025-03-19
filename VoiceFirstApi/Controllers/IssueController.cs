using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoiceFirstApi.DtoModels;
using VoiceFirstApi.IService;
using VoiceFirstApi.Models;
using VoiceFirstApi.Service;

namespace VoiceFirstApi.Controllers
{
    [Route("api/issue")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        public IssueController(IIssueService IssueService)
        {
            _issueService = IssueService;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAsync([FromForm] IssueDtoModel IssueDto)
        {
            try
            {
                var (data, message, status_code) = await _issueService.AddAsync(IssueDto);

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (data, message, code) = await _issueService.GetAllAsync();

        
       
         

            return Ok(new { data, message, status = code });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var (issue, message, code) = await _issueService.GetByIdAsync(id);

            if (issue == null)
            {
                return NotFound(new { message, status = code });
            }

            return Ok(new { data = issue, message, status = code });
        }
    }
}
