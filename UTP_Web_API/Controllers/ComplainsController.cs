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

        public ComplainsController(IComplainRepository complainRepo, IComplainAdapter complainAdapter,
              IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, IInvestigatorRepository investigatorRepo)
        {
            _complainRepo = complainRepo;
            _complainAdapter = complainAdapter;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
            _investigatorRepo = investigatorRepo;
        }

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
            if (id == 0)
            {
                return BadRequest();
            }
            if (await _complainRepo.ExistAsync(x => x.ComplainId == id) == false)
            {
                return NotFound();
            }
            var complain = await _complainRepo.GetById(id);

            var complainDto = _complainAdapter.Bind(complain);

            return Ok(complainDto);            
        }

        [HttpGet("complains")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetComplainDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComplains()
        {
            var complains = await _complainRepo.All();

            if (complains == null)
            {
                return NotFound();
            }
            return Ok(complains
            .Select(c => _complainAdapter.Bind(c))
            .ToList());
        }

        [HttpPost("complain")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateComplainDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateComplainDto>> CreateComplain(CreateComplainDto complain)
        {
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);


            if (complain == null)
            {
                return BadRequest();
            }
            var user = await _userRepo.GetAsync(u => u.Id == currentUserId);

            var createComplain = _complainAdapter.Bind(complain, user);

            await _complainRepo.CreateAsync(createComplain);

            return Ok();
        }

        [HttpPut("complains/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddInvestigatorToComplain(int id, UpdateComplainDto updateComplainDto)
        {
            if (updateComplainDto == null)
            {
                return BadRequest();
            }
            if (await _complainRepo.ExistAsync(x => x.ComplainId == id) == false)
            {
                return NotFound();
            }
            var foundComplain = await _complainRepo.GetAsync(c => c.ComplainId == id);
            var foundInvestigator = await _investigatorRepo.GetAsync(i => i.InvestigatorId == updateComplainDto.TyrejoId);
            var stage = new InvestigationStage { Stage = updateComplainDto.AtliekamiVeiksmai, TimeStamp = DateTime.Now };
            
            foundComplain.Investigator = foundInvestigator;
            foundComplain.Stages?.Add(stage);

            await _complainRepo.Update(foundComplain);
           // var mapedComplainUpdates = await _complainAdapter.Bind(foundComplain, foundInvestigator, updateComplainDto.AtliekamiVeiksmai);

            return NoContent();

        }
    }
    
}
