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
        /// <param name="model">email and password</param> 
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation("User {date} tried login with {model} email adress", DateTime.Now, model.Email);
                var loginResponse = await _userRepo.LoginAsync(model);


                if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} Login exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Vartotojo regisracija, atliekant patikrinima ar vartotojo su tokiu pat Email nera duomenu bazeje
        /// </summary>
        /// <param name="model">email, name, lastname, password</param>
        /// <returns></returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            try
            {
                _logger.LogInformation("User {date} tried registrate with {model} email adress", DateTime.Now, model.Email);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} Registration exception error.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}



