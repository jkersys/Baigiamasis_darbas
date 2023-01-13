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
        private readonly ILogger<ConclusionController> _logger;


        public ConclusionController(IConclusionRepository conclusionRepo, IConclusionAdapter conclusionAdapter, ILogger<ConclusionController> logger)
        {
            _conclusionRepo = conclusionRepo;
            _conclusionAdapter = conclusionAdapter;
            _logger = logger;
        }
        /// <summary>
        /// Grazina visas galimas isvadas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("conclusions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetConclusionDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllConclusions()
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get all Conclusions");
                var conclusions = await _conclusionRepo.GetAllAsync();

            if (conclusions == null)
            {
                    _logger.LogInformation($"{DateTime.Now} No conclusion found");
                    return NotFound();
            }
            return Ok(conclusions
            .Select(c => _conclusionAdapter.Bind(c))
            .ToList());
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetAllConclusions exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// Grazina viena isvada
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("Conclusion/{id:int}", Name = "GetConclusion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetConclusionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<GetConclusionDto>> GetConclusionById(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get conclusion {id}");
                if (id == 0)
                {
                    _logger.LogInformation($"{DateTime.Now} input {id} is not valid");
                    return BadRequest();
                }
                var conclusion = await _conclusionRepo.GetAsync(x => x.ConclusionId == id);
                if (conclusion == null)
                {
                    _logger.LogInformation($"{DateTime.Now} Conclusion Nr. {id} not exist");
                    return NotFound();
                }

                return Ok(_conclusionAdapter.Bind(conclusion));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} GetConclusionById exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Atnaujinamas isvados tekstas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateConclusionDto"></param>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("conclusion/update/{id:int}")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateConclusion(int id, AddOrUpdateConclusionDto updateConclusionDto)
        {

            try
            {
                _logger.LogInformation($"{DateTime.Now} atempt to get conclusion {id}");

                if (id == 0 || updateConclusionDto == null)
                {
                    _logger.LogInformation($"{DateTime.Now} input {id} or {updateConclusionDto} not valid");
                    return BadRequest();
                }
                if (await _conclusionRepo.ExistAsync(x => x.ConclusionId == id) == false)
                {
                    _logger.LogInformation($"{DateTime.Now} Conclusion Nr {id} not found");
                    return NotFound();
                }
                var foundConclusion = await _conclusionRepo.GetAsync(c => c.ConclusionId == id);

                foundConclusion.Decision = updateConclusionDto.Conclusion;

                await _conclusionRepo.Update(foundConclusion);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} UpdateConclusion exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// I duomenu baze irasoma nauja isvada
        /// </summary>
        /// <param name="CreateConclusionDto"></param>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>        
        [HttpPost("complain")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> CreateConclusion(AddOrUpdateConclusionDto CreateConclusionDto)
        {
            try 
            { 
            if (CreateConclusionDto == null)
            {
                return BadRequest();
            }

            var createConclusion = new Conclusion() { Decision = CreateConclusionDto.Conclusion };
            //var createConclusion = _conclusionAdapter.Bind(isvada);

            await _conclusionRepo.CreateAsync(createConclusion);

            return CreatedAtRoute("GetInvestigation", new { id = createConclusion.ConclusionId }, _conclusionAdapter.Bind(createConclusion));
        }
             catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} CreateInvestigator exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Istrinama isvada is duomenu bazes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  <response code="204">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("conclusion/delete/{id:int}")]
        //[Authorize(Roles = "super-admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteConclusion(int id)
        {
            try
            {
                _logger.LogInformation($"{DateTime.Now} attempt to delete conclusion {id}.");
                if (id == 0)
            {
                return BadRequest();
            }

            var conclusion = await _conclusionRepo.GetAsync(d => d.ConclusionId == id);

            if (conclusion == null)
            {
                    _logger.LogInformation($"{DateTime.Now} Conclusion Nr. {id} not found");
                    return NotFound();
            }

            await _conclusionRepo.RemoveAsync(conclusion);

            return NoContent();
        }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} DeleteConclusion exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
