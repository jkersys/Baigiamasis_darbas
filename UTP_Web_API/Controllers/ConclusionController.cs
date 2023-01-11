using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ConclusionDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConclusionController : ControllerBase
    {
        private readonly IConclusionRepository _conclusionRepo;
        private readonly IConclusionAdapter _conclusionAdapter;

        public ConclusionController(IConclusionRepository conclusionRepo, IConclusionAdapter conclusionAdapter)
        {
            _conclusionRepo = conclusionRepo;
            _conclusionAdapter = conclusionAdapter;
        }

        [HttpGet("conclusions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComplains()
        {
            var conclusions = await _conclusionRepo.GetAllAsync();

            if (conclusions == null)
            {
                return NotFound();
            }
            return Ok(conclusions
            .Select(c => _conclusionAdapter.Bind(c))
            .ToList());
        }

        [HttpGet("Conclusion/{id:int}", Name = "GetConclusion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetConclusionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<GetConclusionDto>>GetConclusionById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var conclusion = await _conclusionRepo.GetAsync(x => x.ConclusionId == id);
            if (conclusion == null)
            {
                return NotFound();
            }
            //var conclusionMap = _conclusionAdapter.Bind(conclusion);

            //var complainDto = _conclusionRepo.Bind(complain);

            return Ok(_conclusionAdapter.Bind(conclusion));
        }

        [HttpPut("conclusion/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddConclusion(int id, string isvada)
        {
            if (id == 0 || isvada == null)
            {
                return BadRequest();
            }
            if (await _conclusionRepo.ExistAsync(x => x.ConclusionId == id) == false)
            {
                return NotFound();
            }
            var foundConclusion = await _conclusionRepo.GetAsync(c => c.ConclusionId == id);

            foundConclusion.Decision = isvada;
           
            await _conclusionRepo.Update(foundConclusion);

            return NoContent();

        }

        [HttpPost("complain")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> CreateConclusion(string isvada)
        {         

            if (isvada == null)
            {
                return BadRequest();
            }

            var createConclusion = new Conclusion() { Decision = isvada };
            //var createConclusion = _conclusionAdapter.Bind(isvada);

            await _conclusionRepo.CreateAsync(createConclusion);

            return Ok();
        }

        [HttpDelete("conclusion/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteConclusion(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var conclusion = await _conclusionRepo.GetAsync(d => d.ConclusionId == id);

            if (conclusion == null)
            {
                return NotFound();
            }

            await _conclusionRepo.RemoveAsync(conclusion);

            return NoContent();
        }


    }
}
