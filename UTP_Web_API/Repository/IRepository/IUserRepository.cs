using System.Linq.Expressions;
using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> IsUniqueUserAsync(string username);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<LocalUser> RegisterAsync(RegistrationRequest registrationRequest);
        Task<bool> ExistAsync(int userId);
        bool TryLogin(string userName, string password, out LocalUser? user);
        Task<LocalUser> GetAsync(Expression<Func<LocalUser, bool>> filter);
    }
}
