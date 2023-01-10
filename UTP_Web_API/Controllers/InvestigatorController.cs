using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models.Dto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigatorController : ControllerBase
    {
        private readonly IInvestigatorRepository _investigatorRepo;
        private readonly IInvestigatorAdapter _iAdapter;
        public InvestigatorController(IInvestigatorRepository investigatorRepo, IInvestigatorAdapter iAdapter)
        {
            _investigatorRepo = investigatorRepo;
            _iAdapter = iAdapter;
        }

        [HttpPost("investigator")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<CreateInvestigatorDto>> CreateComplain(CreateInvestigatorDto investigator)
        {
            if (investigator == null)
            {
                return BadRequest();
            }

            //sumapinam kad tiktu duombazei is front endo paduodamas naujas Dish
            var createComplain = _iAdapter.Bind(investigator);

            await _investigatorRepo.CreateAsync(createComplain);

            return Ok();
        }

        [HttpGet("investigator/{id:int}", Name = "GetInvestigator")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetInvestigatorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetInvestigatorDto>> GetDishById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var investigator = await _investigatorRepo.GetAsync(i => i.InvestigatorId == id);

            if (investigator == null)
            {
                return NotFound();
            }

            var investigator1 = _iAdapter.Bind(investigator);
            return Ok(investigator);
        }
    }
}
