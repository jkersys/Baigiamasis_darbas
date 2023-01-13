using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ComplainDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplainsController : ControllerBase
    {
        private readonly IComplainRepository _complainRepo;
        private readonly IComplainAdapter _complainAdapter;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepo;
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly ILogger<ComplainsController> _logger;


        public ComplainsController(IComplainRepository complainRepo, IComplainAdapter complainAdapter,
              IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, IInvestigatorRepository investigatorRepo, IInvestigationStagesRepository stagesRepo, ILogger<ComplainsController> logger)
        {
            _complainRepo = complainRepo;
            _complainAdapter = complainAdapter;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _investigatorRepo = investigatorRepo;
            _logger = logger;
        }

        /// <summary>
        /// Grazina viena skunda su pilnais duomenimis
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        //[Authorize(Roles = "Customer")]
        [HttpGet("complains/{id:int}", Name = "GetComplain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetComplainDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<GetComplainDto>> GetComplainById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get complain id {id} ");
                if (id == 0)
            {
                    _logger.LogInformation($"{DateTime.Now}input {id} not exist ");
                    return BadRequest();
            }
            if (await _complainRepo.ExistAsync(x => x.ComplainId == id) == false)
            {
                    _logger.LogInformation($"{DateTime.Now} complain Nr. {id} not exist ");
                    return NotFound();
            }
            var complain = await _complainRepo.GetById(id);
            var complainDto = _complainAdapter.Bind(complain);

            return Ok(complainDto);            
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetComplainById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Grazina visus tyrimus, atvaizduojant tik dali duomenu
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("complains")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetComplainDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComplains()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to get all complains");
                var complains = await _complainRepo.All();

            if (complains == null)
            {
                    _logger.LogInformation($"{DateTime.Now} complains not found");
                    return NotFound();
            }
            return Ok(complains
            .Select(c => _complainAdapter.Bind(c))
            .ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetComplains exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Sukuriamas naujas tyrimas
        /// </summary>
        /// <param name="CreateComplainDto"></param>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("complain")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetComplainDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetComplainDto>> CreateComplain(CreateComplainDto complain)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to create complains");

                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);

            if (complain == null)
            {
                    _logger.LogInformation($"{DateTime.Now} invalid input {complain}");
                    return BadRequest();
            }
            var user = await _userRepo.GetAsync(u => u.Id == currentUserId);
            if (user == null)
            {
                    _logger.LogInformation($"{DateTime.Now} user not exist");
                    return NotFound();
            }
            var createComplain = _complainAdapter.Bind(complain, user);

            await _complainRepo.CreateAsync(createComplain);

                return CreatedAtRoute("GetComplain", new { id = createComplain.ComplainId }, _complainAdapter.Bind(createComplain));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateComplain exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpPut("complains/{id:int}/conclusion/update/{id:int}")]
        /// <summary>
        /// Pridedamas tyrejas prie complain
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateComplainDto"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("complains/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddInvestigatorToComplain(int id, UpdateComplainDto updateComplainDto)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} Attempt to add investigator to complain");
                if (id == 0 || updateComplainDto == null)
            {
                    _logger.LogInformation($"{DateTime.Now} input {id} or {updateComplainDto} not valid");
                    return BadRequest();
            }
            if (await _complainRepo.ExistAsync(x => x.ComplainId == id) == false)
            {
                    _logger.LogInformation($"{DateTime.Now} complain Nr {id} not found");
                    return NotFound();
            }
            var foundComplain = await _complainRepo.GetAsync(c => c.ComplainId == id);
            var foundInvestigator = await _investigatorRepo.GetAsync(i => i.InvestigatorId == updateComplainDto.TyrejoId);
                if(foundInvestigator == null)
                {
                    _logger.LogInformation($"{DateTime.Now} investigator {updateComplainDto.TyrejoId} not exist");
                    return NotFound();
                }
                var stage = new InvestigationStage { Stage = updateComplainDto.AtliekamiVeiksmai, 
                TimeStamp = DateTime.Now,
            };

            foundComplain.Investigator = foundInvestigator;
            foundComplain.Stages.Add(stage);

            await _complainRepo.Update(foundComplain);

            return NoContent();
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} UpdateInvestigation exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Istrinamas complain is duomenu bazes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  <response code="204">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("complains/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteComplain(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to delete complain with id {id} ");
                if (id == 0)
            {
                    _logger.LogInformation($"{DateTime.Now} invalid input {id}");
                    return BadRequest();
            }

            var complain = await _complainRepo.GetAsync(d => d.ComplainId == id);

            if (complain == null)
            {
                    _logger.LogInformation($"{DateTime.Now} complain with id {id} not found");
                    return NotFound();
            }
            complain.Stages.Clear();
            await _complainRepo.RemoveAsync(complain);

            return NoContent();
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} UpdateInvestigation exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
