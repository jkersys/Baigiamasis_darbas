using UTP_Web_API.Models;
using UTP_Web_API.Services;

namespace UTP_Web_API.Database.InitialData
{
    public class LocalUserInitialData
    {
        public static LocalUser[] GetDataSeed()
        {
            var passwordService = new PasswordService();
            byte[] passwordSalt;
            byte[] passwordHash;

            passwordService.CreatePasswordHash("admin", out passwordHash, out passwordSalt);

            return new LocalUser[] {
                new LocalUser
                {
                    Id = 1,
                    Email = "admin@admin.lt",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = 866666666,
                    Role = UserRole.Admin,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,

                },
            };
        }
    }
}
