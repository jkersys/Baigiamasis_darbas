using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.LocalUserDto;
using UTP_Web_API.Repository.IRepository;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UtpContext _db;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public UserRepository(UtpContext db, IPasswordService passwordService, IJwtService jwtService)
        {
            _db = db;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<bool> ExistAsync(int userId)
        {
            var isRegistered = await _db.LocalUser.AnyAsync(u => u.Id == userId);
            return isRegistered;
        }

        /// <summary>
        /// Should return a flag indicating if a user with a specified username already exists
        /// </summary>
        /// <param name="email">Registration username</param>
        /// <returns>A flag indicating if username already exists</returns>
        public async Task<bool> IsUniqueUserAsync(string email)
        {
            var user = await _db.LocalUser.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var inputPasswordBytes = Encoding.UTF8.GetBytes(loginRequest.Password);
            var user = await _db.LocalUser.FirstOrDefaultAsync(x => x.Email.ToLower() == loginRequest.Email.ToLower());

            if (user == null || !_passwordService.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new LoginResponse
                {
                    Token = "",
                    Email = null
                };
            }

            var token = _jwtService.GetJwtToken(user.Id, user.Role.ToString());

            LoginResponse loginResponse = new()
            {
                Token = token,
                Email = user.Email
            };

            return loginResponse;
        }


        // Add RegistrationResponse (Should not include password)
        // Add adapter classes to map to wanted classes
        public async Task<LocalUser> RegisterAsync(RegistrationRequest registrationRequest)
        {
            _passwordService.CreatePasswordHash(registrationRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            LocalUser user = new()
            {
                Email = registrationRequest.Email,
                FirstName = registrationRequest.Name,
                LastName = registrationRequest.LastName,
                PhoneNumber = registrationRequest.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _db.LocalUser.Add(user);
            await _db.SaveChangesAsync();
            user.PasswordHash = null;
            return user;
        }


        public async Task<LocalUser> GetAsync(Expression<Func<LocalUser, bool>> filter)
        {
            var user = await _db.LocalUser.FirstOrDefaultAsync(filter);

            return user;
        }

        public async Task<LocalUser> UpdateAsync(LocalUser user)
        {

            _db.Update(user);
            await _db.SaveChangesAsync();

            return user;


        }

        public async Task<LocalUser> GetUser(string email)
        {
            return await _db.LocalUser.Include(x => x.Investigator).FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<LocalUser> GetUserById(int id)
        {
            return await _db.LocalUser.Include(x => x.Investigator).FirstOrDefaultAsync(x => x.Id == id);
        }



    }
}