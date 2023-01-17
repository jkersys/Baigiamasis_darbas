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
    public class AdministrativeInspectionStageController : ControllerBase
    {
            private readonly IAdministrativeInspectionRepository _adminInspectionRepo;
            private readonly ILogger<AdministrativeInspectionStageController> _logger;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public AdministrativeInspectionStageController(IAdministrativeInspectionRepository adminInspectionRepo, ILogger<AdministrativeInspectionStageController> logger, IHttpContextAccessor httpContextAccessor)
            {
                _adminInspectionRepo = adminInspectionRepo;
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
        [HttpPut("inspection/stage/update/{id:int}")]
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
                _logger.LogInformation($"{DateTime.Now} atempt to add stage to inspection");
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
                if (await _adminInspectionRepo.ExistAsync(x => x.AdministrativeInspectionId == id) == false)
                {
                    _logger.LogInformation($"{DateTime.Now} complain id {id} not exist");
                    return NotFound();
                }
                var foundInspection = await _adminInspectionRepo.GetAsync(x => x.AdministrativeInspectionId == id);
                var newStage = new InvestigationStage { Stage = stage.Stage, TimeStamp = DateTime.Now };
                foundInspection.InvestigationStages?.Add(newStage);

                await _adminInspectionRepo.Update(foundInspection);

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
