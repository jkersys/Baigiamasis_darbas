using Microsoft.AspNetCore.Authorization;
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
    public class InvestigationStageController : ControllerBase
    {
        private readonly IInvestigationRepository _investigationRepo;
        private readonly ILogger<InvestigationStageController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public InvestigationStageController(IInvestigationRepository investigationRepo, ILogger<InvestigationStageController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _investigationRepo = investigationRepo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        //
        /// <summary>
        /// prie tyrimo pridedamas naujas etapas(istorija)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("investigation/stage/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult> AddStagesToComplain(int id, AddNewStage stage)
        {

            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to add stage to investigation");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add investigator to complain");
                    return BadRequest();
                }
                if (stage.Stage == null)
                {
                    _logger.LogInformation($"{DateTime.Now} input {stage} not valid");
                    return BadRequest();
                }
                if (await _investigationRepo.ExistAsync(x => x.InvestigationId == id) == false)
                {
                    _logger.LogInformation($"{DateTime.Now} complain id {id} not exist");
                    return NotFound();
                }
                var foundInvestigation = await _investigationRepo.GetAsync(x => x.InvestigationId == id);
                var newStage = new InvestigationStage { Stage = stage.Stage, TimeStamp = DateTime.Now };
                foundInvestigation.Stages?.Add(newStage);

                await _investigationRepo.Update(foundInvestigation);

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
    

