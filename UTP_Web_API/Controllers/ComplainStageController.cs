using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainStageController : ControllerBase
    {
        private readonly IComplainRepository _complainRepo;
        private readonly ILogger<ComplainStageController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComplainStageController(IComplainRepository complainRepo, ILogger<ComplainStageController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _complainRepo = complainRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        //
        /// <summary>
        /// prie skundo pridedamas naujas etapas(istorija)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("investigator/complains/stage/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddStagesToComplain(int id, AddNewStage stage)
        {

            try
            {             
                _logger.LogInformation($"{DateTime.Now} atempt to add stage to complain");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add investigator to complain");
                    return BadRequest();
                }
                if (stage == null)
            {
                    _logger.LogInformation($"{DateTime.Now} input {stage} not valid");
                    return BadRequest();
            }
            if (await _complainRepo.ExistAsync(x => x.ComplainId == id) == false)
            {
                    _logger.LogInformation($"{DateTime.Now} complain id {id} not exist");
                    return NotFound();
            }
            var foundComplain = await _complainRepo.GetAsync(x => x.ComplainId == id);                               
            var newStage = new InvestigationStage { Stage = stage.Stage, TimeStamp = DateTime.Now };          
            foundComplain.Stages?.Add(newStage);

            await _complainRepo.Update(foundComplain);

            return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} DeleteConclusion exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
