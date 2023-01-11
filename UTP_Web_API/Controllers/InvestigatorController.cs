using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
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
        public InvestigatorController(IInvestigatorRepository investigatorRepo, IInvestigatorAdapter iAdapter, IUserRepository userRpo)
        {
            _investigatorRepo = investigatorRepo;
            _iAdapter = iAdapter;
            _userRepo = userRpo;
        }

        [HttpPost("investigator")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<CreateInvestigatorDto>> CreateInvestigator(CreateInvestigatorDto investigator)
        {
            if (investigator == null)
            {
                return BadRequest();
            }

            var user = await _userRepo.GetUser(investigator.VartotojoElPastas);

            if (user == null)
            {
                return NotFound($"User email {investigator.VartotojoElPastas} not found");
            }
            var createInvestigator = _iAdapter.Bind(investigator, user);

            await _investigatorRepo.CreateAsync(createInvestigator);

            return Ok();
        }

        [HttpGet("investigator/{id:int}", Name = "GetInvestigator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetInvestigatorDto>> InvestigatorById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var investigator = await _investigatorRepo.GetById(id);

            if (investigator == null)
            {
                return NotFound();
            }            
            return Ok(_iAdapter.Bind(investigator));
        }
    }
}
