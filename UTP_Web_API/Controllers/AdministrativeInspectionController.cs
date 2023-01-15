using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.AdminInspection;
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


        public AdministrativeInspectionController(IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, 
           IAdministrativeInspectionRepository adminInspectionRepo, ICompanyRepository companyRepo, IAdministrativeInspectionAdapter inspectionAdapter, ILogger<AdministrativeInspectionController> logger)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _adminInspectionRepo = adminInspectionRepo;
            _companyRepo = companyRepo;
            _inspectionAdapter = inspectionAdapter;
            _logger = logger;
        }

        /// <summary>
        /// sukuriama nauja administracine patikra
        /// </summary>
        /// <param name="inspection"></param>
        /// <returns></returns>
        [HttpPost("inspection")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetOneAdministrativeInspectionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetOneAdministrativeInspectionDto>> CreateAdministrativeInspection(CreateAdminInspectionDto inspection)
        {
            try
            {
                _logger.LogError($"{DateTime.Now} attempt to create new administrative inspection.");
                if (inspection == null)
            {
                    _logger.LogError($"{DateTime.Now} input {inspection} not valid");
                    return BadRequest();
            }
            //null klaida 500 jeigu neprisijunges
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
                Stage = inspection.InvestigationStages,
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
        public async Task<ActionResult<GetOneAdministrativeInspectionDto>> GetInspectionById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get one administrative inspection {id}");
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
                var inspection = await _adminInspectionRepo.All();

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
    }
}
