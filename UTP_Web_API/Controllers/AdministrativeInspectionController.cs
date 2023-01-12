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
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IAdministrativeInspectionRepository _adminInspectionRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAdministrativeInspectionAdapter _inspectionAdapter;


        public AdministrativeInspectionController(IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, 
            IInvestigatorRepository investigatorRepo, IAdministrativeInspectionRepository adminInspectionRepo, ICompanyRepository companyRepo, IAdministrativeInspectionAdapter inspectionAdapter)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _investigatorRepo = investigatorRepo;
            _adminInspectionRepo = adminInspectionRepo;
            _companyRepo = companyRepo;
            _inspectionAdapter = inspectionAdapter;
        }


        [HttpPost("complain")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetOneAdministrativeInspectionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetOneAdministrativeInspectionDto>> CreateAdministrativeInspection(CreateAdminInspectionDto inspection)
        {
            if (inspection == null)
            {
                return BadRequest();
            }
            //null klaida 500 jeigu neprisijunges
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            var user = await _userRepo.GetUserById(currentUserId);
            var foundInvestigator = user.Investigator;
            var foundCompany = await _companyRepo.GetAsync(i => i.CompanyId == inspection.CompanyId);
            if (foundInvestigator == null || foundCompany == null)
            {
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

            return Ok();
        }

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
            if (id == 0)
            {
                return BadRequest();
            }
            if (await _adminInspectionRepo.ExistAsync(x => x.AdministrativeInspectionId == id) == false)
            {
                return NotFound();
            }
            var inspection = await _adminInspectionRepo.GetById(id);

            var inspectionDto = _inspectionAdapter.BindOneInspection(inspection);

            return Ok(inspectionDto);
        }

        [HttpGet("inspections")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAdministrativeInspectionsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInvestigations()
        {
            var inspection = await _adminInspectionRepo.All();

            if (inspection == null)
            {
                return NotFound();
            }
            return Ok(inspection
            .Select(c => _inspectionAdapter.Bind(c))
            .ToList());
        }
    }
}
