using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.LocalUserDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepo, IJwtService jwtService, ILogger<UserController> logger)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _logger = logger;
        }


        /// <summary>
        /// Tikrina vartotojo prisiloginima, ar sutampa vartotojo email ir password su duomenu bazeje esanciais 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
           _logger.LogInformation("User {date} tried login with {model} email adress",DateTime.Now, model.Email);
            var loginResponse = await _userRepo.LoginAsync(model);

            if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }
                return Ok(loginResponse);
        }
        /// <summary>
        ///Vartotojo regisracija, tikrinama ar vartotojo su tokiu pat Email nera duomenu bazeje 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            var isUserNameUnique = await _userRepo.IsUniqueUserAsync(model.Email);

            if (!isUserNameUnique)
            {
                return BadRequest(new { message = "User already exists" });
            }

            var user = await _userRepo.RegisterAsync(model);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Created(nameof(Login), new { id = user.Id });
            
        }

        [HttpPatch("patch/{id:int}", Name = "UpdateUserRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserRole(int id, JsonPatchDocument<LocalUser> request)
        {
            if (id == 0 || request == null)
            {
                return BadRequest();
            }

            var userExist = await _userRepo.ExistAsync(id);
            if (!userExist)
            {
                return NotFound();
            }

            var foundUser = await _userRepo.GetAsync(d => d.Id == id);

            request.ApplyTo(foundUser, ModelState);

            await _userRepo.UpdateAsync(foundUser);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();


        }

    }
}

