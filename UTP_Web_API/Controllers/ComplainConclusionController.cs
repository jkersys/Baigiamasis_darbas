using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTP_Web_API.Models.Dto.ConclusionDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainConclusionController : ControllerBase
    {   
        private readonly IConclusionRepository _conclusionRepo;
        private readonly IComplainRepository _complainRepo;
        private readonly ILogger<ComplainConclusionController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComplainConclusionController(IComplainRepository complainRepo,
               IConclusionRepository conclusionRepo, ILogger<ComplainConclusionController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _complainRepo = complainRepo;
            _conclusionRepo = conclusionRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Pridedama isvada prie complain
        /// </summary>
        /// <param name="complainId"></param>
        /// <param name="conclusion"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("complains/{complainId:int}/conclusions")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddConclusionToComplain(int complainId, AddConclusionToCaseDto conclusion)
        {
            try
            {
                    _logger.LogInformation($"{DateTime.Now} attempt to add conclusion to complain ");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add conclusion to complain");
                    return BadRequest();
                }
                if (conclusion == null || complainId == 0)
            {
                    _logger.LogInformation($"{DateTime.Now} input {complainId} Or {conclusion} not valid");
                    return BadRequest();
            }
           
            var foundComplain = await _complainRepo.GetById(complainId);
            var foundConclusion = await _conclusionRepo.GetAsync(i => i.ConclusionId == conclusion.ConclusionId);

            if (foundComplain == null || foundConclusion == null)
            {
                    _logger.LogInformation($"{DateTime.Now} complain Nr. {complainId} or conclusion {conclusion.ConclusionId} not found");
                    return NotFound();
            }

            if (foundComplain.Conclusion != null && currentUserRole != "Admin" )
            {
                    _logger.LogInformation($"{DateTime.Now} complain already have conclusion");
                    return BadRequest("Byla jau užbaigta");
            }

            foundComplain.Conclusion = foundConclusion;           
            foundComplain.EndDate = DateTime.Now;
           
            await _complainRepo.Update(foundComplain);

            return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} AddConclusionToComplain exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
