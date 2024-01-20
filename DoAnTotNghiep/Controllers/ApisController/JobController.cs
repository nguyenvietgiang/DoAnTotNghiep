using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.ResponseDTO;
using DoAnTotNghiep.Repository.JobRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.ApisController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public JobController (IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobPostings()
        {
            try
            {
                var jobPostings = await _jobPostingRepository.GetUnapprovedJobPostingsAsync();

                var jobPostingsDto = jobPostings.Select(jp => new JobPostingResponseDto
                {
                    JobId = jp.JobPostingID,
                    Title = jp.Title,
                    Location = jp.Location,
                    Position = jp.position,
                    Company = jp.Employer.CompanyName,
                    Image = jp.Employer.UrlImage
                });

                return Ok(jobPostingsDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobPostingById(Guid id)
        {
            try
            {
                var jobPosting = await _jobPostingRepository.GetJobPostingByIdAsync(id);

                if (jobPosting == null)
                    return NotFound();

                return Ok(jobPosting);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobPosting([FromBody] JobPosting jobPosting)
        {
            try
            {
                await _jobPostingRepository.CreateJobPostingAsync(jobPosting);
                return CreatedAtAction(nameof(GetJobPostingById), new { id = jobPosting.JobPostingID }, jobPosting);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobPosting(Guid id, [FromBody] JobPosting jobPosting)
        {
            try
            {
                if (id != jobPosting.JobPostingID)
                    return BadRequest("ID mismatch");

                await _jobPostingRepository.UpdateJobPostingAsync(jobPosting);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobPosting(Guid id)
        {
            try
            {
                await _jobPostingRepository.DeleteJobPostingAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ToggleJobStatus(Guid id)
        {
            try
            {
                await _jobPostingRepository.ToggleJobStatusAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("positions/count")]
        public async Task<IActionResult> GetJobPositionsCount()
        {
            try
            {
                var positionsCount = await _jobPostingRepository.GetJobPositionsCountAsync();
                return Ok(positionsCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
