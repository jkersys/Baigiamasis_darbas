using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.AdminInspection;
using UTP_Web_API.Models.Dto.ConclusionDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrativeInspectionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyRepository _companyRepo;
        private readonly IAdministrativeInspectionRepository _adminInspectionRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAdministrativeInspectionAdapter _inspectionAdapter;
        private readonly ILogger<AdministrativeInspectionController> _logger;
        private readonly IConclusionRepository _conclusionRepo;



        public AdministrativeInspectionController(IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, 
           IAdministrativeInspectionRepository adminInspectionRepo, ICompanyRepository companyRepo, IAdministrativeInspectionAdapter inspectionAdapter, ILogger<AdministrativeInspectionController> logger, IConclusionRepository conclusionRepo)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _adminInspectionRepo = adminInspectionRepo;
            _companyRepo = companyRepo;
            _inspectionAdapter = inspectionAdapter;
            _logger = logger;
            _conclusionRepo = conclusionRepo;
        }

        /// <summary>
        /// sukuriama nauja administracine patikra
        /// </summary>
        /// <param name="inspection"></param>
        /// <returns></returns>
        [HttpPost("inspection/create")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetOneAdministrativeInspectionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<GetOneAdministrativeInspectionDto>> CreateAdministrativeInspection(CreateAdminInspectionDto inspection)
        {
            try
            {
                _logger.LogError($"{DateTime.Now} attempt to create new administrative inspection.");
                _logger.LogInformation($"{DateTime.Now} attempt to create inspection");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director" && currentUserRole != "Investigator")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to create inspections data");
                    return BadRequest("You have no right to delete companies data");
                }
                if (inspection == null)
            {
                    _logger.LogError($"{DateTime.Now} input {inspection} not valid");
                    return BadRequest();
            }
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            var user = await _userRepo.GetUserById(currentUserId);
            var foundInvestigator = user.Investigator;
            var foundCompany = await _companyRepo.GetAsync(i => i.CompanyId == inspection.CompanyId);
            if (foundInvestigator == null || foundCompany == null)
            {
                    _logger.LogError($"{DateTime.Now} user is not investigator or company id {inspection.CompanyId} not exist");
                    return BadRequest();
            }
            var stage = new InvestigationStage
            {
                Stage = inspection.InvestigationStage,
                TimeStamp = DateTime.Now,
            };
            var newAdministrativeInspection = new AdministrativeInspection()
            {
                StartDate = DateTime.Now,
                Company = foundCompany,                
            };
            newAdministrativeInspection.InvestigationStages.Add(stage);
            newAdministrativeInspection.Investigators.Add(foundInvestigator);
            await _adminInspectionRepo.CreateAsync(newAdministrativeInspection);

                return CreatedAtRoute("GetAdministrativeInspection", new { id = newAdministrativeInspection.AdministrativeInspectionId }, _inspectionAdapter.Bind(newAdministrativeInspection));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateAdministrativeInspection exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// grazina viena administracine patikra su pilnais duomenimis
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response> 
        //[Authorize(Roles = "Customer")]
        [HttpGet("investigation/{id:int}", Name = "GetAdministrativeInspection")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOneAdministrativeInspectionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize]
        public async Task<ActionResult<GetOneAdministrativeInspectionDto>> GetInspectionById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get one administrative inspection {id}");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director" && currentUserRole != "Investigator")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to get inspection data");
                    return BadRequest("You have no right to get inspection data");
                }
                if (id == 0)
            {
                    _logger.LogInformation($"{DateTime.Now} input {id} not valid");
                    return BadRequest();
            }
            if (await _adminInspectionRepo.ExistAsync(x => x.AdministrativeInspectionId == id) == false)
            {
                    _logger.LogInformation($"{DateTime.Now} administrative inspection id Nr. {id} not found");
                    return NotFound();
            }
            var inspection = await _adminInspectionRepo.GetById(id);

            var inspectionDto = _inspectionAdapter.BindOneInspection(inspection);

            return Ok(inspectionDto);
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetInspectionById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// grazina visas administracines patikras
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("inspections")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAdministrativeInspectionsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdminInspections()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get all administrative inspections");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director" && currentUserRole != "Investigator")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to get inspections data");
                    return BadRequest("You have no right to get inspections data");
                }
                var inspection = await _adminInspectionRepo.LoggedUserAdministrativeInspecitons();



                if (inspection == null)
                {
                    _logger.LogInformation($"{DateTime.Now} administrative inspections not found");
                    return NotFound();
                }
                return Ok(inspection
                .Select(c => _inspectionAdapter.Bind(c))
                .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetAdminInspections exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

            /// <summary>
            /// Pridedama isvada prie administracines patikros
            /// </summary>
            /// <param name="inspectionId"></param>
            /// <param name="conclusion"></param>
            /// <returns></returns>
            /// <response code="204">No Content</response>
            /// <response code="400">Bad request</response>
            /// <response code="404">Not Found</response>
            /// <response code="500">Internal server error</response>
            [HttpPut("inspection/{inspectionId:int}/conclusions")]
            // [Authorize(Roles = "admin")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            [Authorize]
            public async Task<ActionResult> AddConclusionToInspection(int inspectionId, AddConclusionToCaseDto conclusion)
            {
                try
                {
                    _logger.LogInformation($"{DateTime.Now} attempt to add conclusion to inspection ");
                    var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    if (currentUserRole != "Investigator" && currentUserRole != "Admin" && currentUserRole != "Director")
                    {
                        _logger.LogInformation($"{DateTime.Now} User have no rights to add conclusion to inspection");
                        return BadRequest();
                    }
                    if (conclusion == null || inspectionId == 0)
                    {
                        _logger.LogInformation($"{DateTime.Now} input {inspectionId} Or {conclusion} not valid");
                        return BadRequest();
                    }

                    var foundComplain = await _adminInspectionRepo.GetById(inspectionId);
                    var foundConclusion = await _conclusionRepo.GetAsync(i => i.ConclusionId == conclusion.ConclusionId);

                    if (foundComplain == null || foundConclusion == null)
                    {
                        _logger.LogInformation($"{DateTime.Now} inspection Nr. {inspectionId} or conclusion {conclusion.ConclusionId} not found");
                        return NotFound();
                    }

                    if (foundComplain.Conclusion != null && currentUserRole != "Admin")
                    {
                        _logger.LogInformation($"{DateTime.Now} inspection already have conclusion");
                        return BadRequest("Byla jau užbaigta");
                    }

                    foundComplain.Conclusion = foundConclusion;
                    foundComplain.EndDate = DateTime.Now;

                    await _adminInspectionRepo.Update(foundComplain);

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{DateTime.Now} AddConclusionToInspection exception error.");
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }

