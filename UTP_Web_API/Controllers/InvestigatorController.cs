using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
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
        public InvestigatorController(IInvestigatorRepository investigatorRepo, IInvestigatorAdapter iAdapter, IUserRepository userRpo)
        {
            _investigatorRepo = investigatorRepo;
            _iAdapter = iAdapter;
            _userRepo = userRpo;
        }
        /// <summary>
        /// sukuria nauja tyreja sujungiant investigator su localuser ir pakeicia role i investigator
        /// </summary>
        /// <param name="investigator"></param>
        /// <returns></returns>
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
            if(user.Investigator != null)
            {
                return BadRequest("Tyrejas tokiu vardu jau yra");
            }
            var createInvestigator = _iAdapter.Bind(investigator, user);
            user.Role = (UserRole)Enum.Parse(typeof(UserRole), "Investigator");
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

        /// <summary>
        /// Ištrina tyrėja iš duomenu bazės, jeigu jis neturi bylu.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("investigator/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteInvestigator(int id)
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

            if (investigator.Complains.Count() != 0 || investigator.AdministrativeInspections.Count != 0 || investigator.Investigations.Count() != 0)
            {
                return BadRequest("Investigator have cases");
            }
            await _investigatorRepo.RemoveAsync(investigator);

            return NoContent();
        }
    }
}
