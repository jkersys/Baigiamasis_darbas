using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigatorDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigatorController : ControllerBase
    {
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly IInvestigatorAdapter _iAdapter;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<InvestigatorController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public InvestigatorController(IInvestigatorRepository investigatorRepo, IInvestigatorAdapter iAdapter, IUserRepository userRpo, ILogger<InvestigatorController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _investigatorRepo = investigatorRepo;
            _iAdapter = iAdapter;
            _userRepo = userRpo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// sukuria nauja tyreja sujungiant investigator su localuser ir pakeicia role i investigator
        /// </summary>
        /// <param name="investigator">pazymejimo nr, kabineto nr, darbovietes adr.,vartotojo el. pastas</param>
        /// <returns></returns>     
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("create")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<GetInvestigatorDto>> CreateInvestigator(CreateInvestigatorDto investigator)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} was atempted to create new investigator");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to create investigator");
                    return BadRequest("You have no right to create investigator");
                }

                if (investigator == null)
                {
                    return BadRequest();
                }
                var user = await _userRepo.GetUser(investigator.LocalUserEmail);
                if (user == null)
                {
                    _logger.LogInformation($"{DateTime.Now} User with {investigator.LocalUserEmail} email was not found");
                    return NotFound($"User email {investigator.LocalUserEmail} not found");
                }
                if (user.Investigator != null)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigator with {investigator.LocalUserEmail} already exist");
                    return BadRequest("Tyrejas tokiu vardu jau yra");
                }
                var createInvestigator = _iAdapter.Bind(investigator, user);
                user.Role = (UserRole)Enum.Parse(typeof(UserRole), "Investigator");
                await _investigatorRepo.CreateAsync(createInvestigator);

                return CreatedAtRoute("GetInvestigator", new { id = createInvestigator.InvestigatorId }, _iAdapter.Bind(createInvestigator));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// grazina visa tyreju sarasa
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("investigators")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetInvestigatorsDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> GetAllInvestigators()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get investigators list");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director" && currentUserRole != "Investigator")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to open investigators list");
                    return BadRequest("You have no right to open investigators list");
                }
                var investigators = await _investigatorRepo.All();

                if (investigators == null)
                {
                    _logger.LogInformation($"{DateTime.Now} investigators not found");
                    return NotFound();
                }
                return Ok(investigators
                .Select(c => new GetInvestigatorsDto(c))
                .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetAllInvestigators exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// grazina viena tyreja su pilnais jo duomenimis
        /// </summary>
        /// <param name="id">id number</param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id:int}", Name = "GetInvestigator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult<GetInvestigatorDto>> InvestigatorById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get investigator with id {id} ");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to open investigator info");
                    return BadRequest("You have no right to open investigator info");
                }
                if (id == 0)
            {
                    _logger.LogInformation($"{DateTime.Now} Input {id} is not valid");
                    return BadRequest();
            }
            var investigator = await _investigatorRepo.GetById(id);

            if (investigator == null)
            {
                _logger.LogInformation($"{DateTime.Now} Investigator with id {id} not exist");
                return NotFound();
            }
            return Ok(_iAdapter.Bind(investigator));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} InvestigatorById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Ištrina tyrėja iš duomenu bazės, jeigu jis neturi bylu.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult> DeleteInvestigator(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to delete investigator with id {id} ");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no rights to delete investigator info");
                    return BadRequest("You have no right to delete investigator");
                }
                if (id == 0)
                {
                    return BadRequest();
                }
                var investigator = await _investigatorRepo.GetById(id);

                if (investigator == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Investigator with id {id} was not found");
                    return NotFound();
                }                
                if (investigator.Complains.Count() != 0 || investigator.AdministrativeInspections.Count != 0 || investigator.Investigations.Count() != 0)
                {
                    _logger.LogWarning($"{DateTime.Now} Atempted to delete investigator Nr. {id}, this investigator have cases");
                    return BadRequest("Investigator have cases");
                }
                await _investigatorRepo.RemoveAsync(investigator);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} DeleteInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// gaunamas tyreju sarasas, kuris naudojamas front ende selectui
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("select")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InvestigatorResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInvestigatorsForFrontEndSelect()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get investigators");
                var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                if (currentUserRole != "Admin" && currentUserRole != "Director")
                {
                    _logger.LogInformation($"{DateTime.Now} User have no right to open investigators list");
                    return BadRequest("You have no right to open investigators list");
                }
                var investigators = await _investigatorRepo.All();

                if (investigators == null)
                {
                    _logger.LogInformation($"{DateTime.Now} investigators not found");
                    return NotFound();
                }
                return Ok(investigators
                .Select(c => _iAdapter.BindForFrontEndResponse(c))
                .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetInvestigators exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
