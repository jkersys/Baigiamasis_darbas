using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ComplainDto;
using UTP_Web_API.Models.Dto.InvestigationDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigationController : ControllerBase
    {
        private readonly IInvestigationRepository _investigatonRepo;
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IInvestigationAdapter _investigationAdapter;
        private readonly ILogger<InvestigationController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public InvestigationController(IInvestigationRepository investigatonRepo, IInvestigatorRepository investigatorRepo, ICompanyRepository companyRepo, IInvestigationAdapter investigationAdapter, ILogger<InvestigationController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _investigatonRepo = investigatonRepo;
            _investigatorRepo = investigatorRepo;
            _companyRepo = companyRepo;
            _investigationAdapter = investigationAdapter;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Grazina viena tyrima su pilnai duomenimis
        /// </summary>
        /// <param name="id">imone, tyrimo etapas, tyrimo pradzios laikas, pabaigos laikas, isvada, tyrejas, baudos dydis </param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        //[Authorize(Roles = "Customer")]
        [HttpGet(("{id}"), Name = "GetInvestigation")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOneInvestigationDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<ActionResult<GetOneInvestigationDto>> GetInvestigationById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get investigation with id {id} ");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to get investigation data");
                    return BadRequest();
                }
                if (id == 0)
                {
                    _logger.LogInformation($"{DateTime.Now} Input {id} is not valid");
                    return BadRequest();
                }
                if (await _investigatonRepo.ExistAsync(x => x.InvestigationId == id) == false)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigation with {id} not found");
                    return NotFound();
                }
                var complain = await _investigatonRepo.GetById(id);

                var complainDto = _investigationAdapter.BindForOneInvestigation(complain);

                return Ok(complainDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetInvestigationById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Grazina visus tyrimus, atvaizduojant tik dali duomenu
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("investigations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetInvestigationsDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllInvestigations()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get all investigations");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add get investigations");
                    return BadRequest();
                }
                var investigations = await _investigatonRepo.All();

                if (investigations == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigations not found");
                    return NotFound();
                }
                return Ok(investigations
                .Select(c => _investigationAdapter.Bind(c))
                .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetAllInvestigations exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// pridedamas i tyrima tyrejas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="addInvestigatorDto"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("investigator/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult> AddInvestigatorToInvestigation(int id, AddInvestigatorDto addInvestigatorDto)
        {
            try 
            {
                _logger.LogInformation($"{DateTime.Now} Atempt to add ivestigator to investigation");

                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

                if (currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add investigator to investigation");
                    return BadRequest();
                }
            if (id == 0 || addInvestigatorDto == null)
            {
                    _logger.LogInformation($"{DateTime.Now} input {id} or {addInvestigatorDto.InvestigatorId} not valid");
                    return BadRequest();
            }
            if (await _investigatonRepo.ExistAsync(x => x.InvestigationId == id) == false)
            {
                    _logger.LogInformation($"{DateTime.Now} Investigation Nr. {id} not exist");
                    return NotFound();
            }         
            var foundInvestigation = await _investigatonRepo.GetById(id);
            var foundInvestigator = await _investigatorRepo.GetAsync(i => i.InvestigatorId == addInvestigatorDto.InvestigatorId);
                if (foundInvestigator == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigator Nr. {addInvestigatorDto.InvestigatorId} not exist");
                    return NotFound();
                }
          
            foundInvestigation.Investigators?.Add(foundInvestigator);

            await _investigatonRepo.Update(foundInvestigation);

            return NoContent();
        }
         catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} AddInvestigatorToInvestigation exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
    }
}

        /// <summary>
        /// Sukuriamas naujas tyrimas
        /// </summary>
        /// <param name="investigationDto"></param>
        /// <param name="updateComplainDto"></param>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetInvestigationsDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<GetInvestigationsDto>> CreateInvestigation(CreateInvestigationDto investigationDto)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} Atempt to create investigation");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to add create investigation");
                    return BadRequest();
                }

                if (investigationDto == null)
                {
                    _logger.LogInformation($"{DateTime.Now} input {investigationDto} not valid");
                    return BadRequest();
                }
                var foundInvestigator = await _investigatorRepo.GetAsync(c => c.InvestigatorId == investigationDto.InvestigatorId);
                var foundCompany = await _companyRepo.GetAsync(i => i.CompanyId == investigationDto.CompanyId);
                var stage = new InvestigationStage
                {
                    Stage = investigationDto.InvestigationStage,
                    TimeStamp = DateTime.Now,
                };
                if (foundInvestigator == null || foundCompany == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Company or investigator is not found in database");
                    return BadRequest();
                }

                var newInvestigation = new Investigation();
                newInvestigation.Company = foundCompany;
                newInvestigation.LegalBase = investigationDto.LegalBase;
                newInvestigation.StartDate = DateTime.Now;
                newInvestigation.Investigators?.Add(foundInvestigator);
                newInvestigation.Stages?.Add(stage);

                await _investigatonRepo.CreateAsync(newInvestigation);
                return CreatedAtRoute("GetInvestigation", new { id = newInvestigation.InvestigationId }, _investigationAdapter.BindForOneInvestigation(newInvestigation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Istrinamas tyrimas is duomenu bazes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  <response code="204">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("investigation/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult> DeleteInvestigation(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to delete investigation with id {id} ");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to delete inspection");
                    return BadRequest();
                }
                if (id == 0)
            {
                    _logger.LogInformation($"{DateTime.Now} invalid input {id}");
                    return BadRequest();
            }
            var investigation = await _investigatonRepo.GetAsync(d => d.InvestigationId == id);

            if (investigation == null)
            {
                    _logger.LogInformation($"{DateTime.Now} investigation with id {id} was not found");
                    return NotFound();
            }
                
                await _investigatonRepo.RemoveAsync(investigation);
            return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} DeleteInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
    

