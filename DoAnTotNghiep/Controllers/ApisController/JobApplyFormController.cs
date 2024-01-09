using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Repository.JobApplyFormRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.ApisController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplyFormController : ControllerBase
    {
        private readonly IJobApplyFormRepository _jobApplyFormRepository;

        public JobApplyFormController(IJobApplyFormRepository jobApplyFormRepository)
        {
            _jobApplyFormRepository = jobApplyFormRepository;
        }

        [HttpPost("AddJobApplyForm")]
        public async Task<IActionResult> AddJobApplyForm([FromBody] JobApplyFormDTO jobApplyFormDTO)
        {
            try
            {
                await _jobApplyFormRepository.AddJobApplyForm(jobApplyFormDTO);
                return Ok("JobApplyForm added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add JobApplyForm. {ex.Message}");
            }
        }

        [HttpGet("jobPosting/{jobPostingID}")]
        public async Task<IActionResult> GetJobApplyFormsByJobPostingID(Guid jobPostingID)
        {
            try
            {
                var jobApplyForms = await _jobApplyFormRepository.GetJobApplyFormsByJobPostingID(jobPostingID);
                return Ok(jobApplyForms);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve JobApplyForms. {ex.Message}");
            }
        }

        [HttpGet("{jobApplyID}")]
        public async Task<IActionResult> GetJobApplyFormById(Guid jobApplyID)
        {
            try
            {
                var jobApplyForm = await _jobApplyFormRepository.GetJobApplyFormById(jobApplyID);
                return Ok(jobApplyForm);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve JobApplyForm. {ex.Message}");
            }
        }


        [HttpDelete("{jobApplyID}")]
        public async Task<IActionResult> DeleteJobApplyForm(Guid jobApplyID)
        {
            try
            {
                await _jobApplyFormRepository.DeleteJobApplyForm(jobApplyID);
                return Ok("JobApplyForm deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete JobApplyForm. {ex.Message}");
            }
        }
    }
}
