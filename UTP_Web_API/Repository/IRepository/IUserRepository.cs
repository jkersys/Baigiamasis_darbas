using System.Linq.Expressions;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.LocalUserDto;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUserAsync(string username);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<LocalUser> RegisterAsync(RegistrationRequest registrationRequest);
        Task<bool> ExistAsync(int userId);
        Task<LocalUser> GetAsync(Expression<Func<LocalUser, bool>> filter);
        Task<LocalUser> UpdateAsync(LocalUser user);
        Task<LocalUser> GetUser(string email);
        Task<LocalUser> GetUserById(int id);
    }
}
