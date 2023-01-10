using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UTP_Web_API.Models.Dto;
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

        public UserController(IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }


        /// <summary>
        /// Tikrina vartotojo prisiloginima, ar sutampa vartotojo email ir password su duomenu bazeje esanciais 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        //padarys async
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
        //    var isOk = _userRepo.TryLogin(model.Email, model.Password, out var user);
        //    if (!isOk)
        //        return Unauthorized("Bad username or password");

        //    var token = _jwtService.GetJwtToken(user.Id, user.Role.ToString());

        //    return Ok(new LoginResponse { Email = model.Email, Token = token });


            var loginResponse = await _userRepo.LoginAsync(model);

            if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }
                return Ok(loginResponse);
        }
        /// <summary>
        ///Vartotojo regisracija 
        /// </summary>
        /// <param name="model"></param>
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

            //return Ok();



            //[ProducesResponseType(StatusCodes.Status201Created)]
            //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
            //[Produces("application/json")]
            //[Consumes("application/json")]
            //[HttpPost("register")]
            //public async Task<IActionResult> ChangeRole([FromBody] RegistrationRequest model)
            //{

            //}
        }
    }
}

