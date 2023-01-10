using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models.Dto;
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
        // private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepo;

        public ComplainsController(IComplainRepository complainRepo, IComplainAdapter complainAdapter,
              IUserRepository userRepo)
        {
            _complainRepo = complainRepo;
            _complainAdapter = complainAdapter;
            // _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
        }


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
            var complain = _complainRepo.GetById(id);

            if (complain == null)
            {
                return NotFound();
            }

            var complainDto = _complainAdapter.Bind(complain);

            return Ok(complainDto);
           // return Ok(complain1);
        }

        [HttpGet("complains")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetComplainDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComplains()
        {
            var complains = _complainRepo.All();

            //var complainers = _complainAdapter.Bind(complains);
            return (Ok(complains));

            //return Ok(complains
            //.Select(d => new GetComplainDto(d))
            //.ToList());
        }


        [HttpPost("complain")]
        // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateComplainDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<CreateComplainDto>> CreateComplain(CreateComplainDto complain)
        {
            if (complain == null)
            {
                return BadRequest();
            }
            var user = await _userRepo.GetAsync(u => u.Id == 2);

            //sumapinam kad tiktu duombazei is front endo paduodamas naujas Dish
            var createComplain = _complainAdapter.Bind(complain, user);

            await _complainRepo.CreateAsync(createComplain);

            return Ok();

            //return CreatedAtRoute("GetDish", new { id = model.DishId }, dishDto);

        }

    }
}
