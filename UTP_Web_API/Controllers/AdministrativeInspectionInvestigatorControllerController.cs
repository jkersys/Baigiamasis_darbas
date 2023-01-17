using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTP_Web_API.Models.Dto.ComplainDto;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeInspectionInvestigatorControllerController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdministrativeInspectionRepository _adminInspectionRepo;
        private readonly ILogger<AdministrativeInspectionInvestigatorControllerController> _logger;
        private readonly IInvestigatorRepository _investigatorRepo;



        public AdministrativeInspectionInvestigatorControllerController(IUserRepository userRepo, IHttpContextAccessor httpContextAccessor,
           IAdministrativeInspectionRepository adminInspectionRepo, ICompanyRepository companyRepo, ILogger<AdministrativeInspectionInvestigatorControllerController> logger, IConclusionRepository conclusionRepo, IInvestigatorRepository investigatorRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _adminInspectionRepo = adminInspectionRepo;
            _logger = logger;
            _investigatorRepo = investigatorRepo;
        }




        //[HttpPut("complains/{id:int}/conclusion/update/{id:int}")]
        /// <summary>
        /// Pridedamas tyrejas prie skundo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addInvestigatorToCoplainDto"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("inspection/investigator/update/{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddInvestigatorToComplain(int id, AddInvestigatorToComplainDto addInvestigatorToInspectionnDto)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} Attempt to add investigator to complain ");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add investigator to inspection");
                    return BadRequest();
                }
                if (id == 0 || addInvestigatorToInspectionnDto == null)
                {
                    _logger.LogInformation($"{DateTime.Now} input {id} or {addInvestigatorToInspectionnDto.InvestigatorId} not valid");
                    return BadRequest();
                }
                if (await _adminInspectionRepo.ExistAsync(x => x.AdministrativeInspectionId == id) == false)
                {
                    _logger.LogInformation($"{DateTime.Now} inspection Nr {id} not found");
                    return NotFound();
                }
                var foundInspection = await _adminInspectionRepo.GetAsync(c => c.AdministrativeInspectionId == id);
                var foundInvestigator = await _investigatorRepo.GetAsync(i => i.InvestigatorId == addInvestigatorToInspectionnDto.InvestigatorId);
                if (foundInvestigator == null)
                {
                    _logger.LogInformation($"{DateTime.Now} investigator {addInvestigatorToInspectionnDto.InvestigatorId} not exist");
                    return NotFound();
                }

                foundInspection.Investigators.Add(foundInvestigator);

                await _adminInspectionRepo.Update(foundInspection);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} AddInvestigatorToComplain exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
