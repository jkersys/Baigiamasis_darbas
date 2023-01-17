using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class AdministrativeInspectionRepository : Repository<AdministrativeInspection>, IAdministrativeInspectionRepository
    {
        private readonly UtpContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdministrativeInspectionRepository(UtpContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<AdministrativeInspection>> LoggedUserAdministrativeInspecitons()
        {
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (currentUserRole == "Admin" || currentUserRole == "Director")
            {
                return await _db.AdministrativeInspection.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.InvestigationStages).ToListAsync();

            }
            else
                return _db.AdministrativeInspection.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion)
                       .Include(x => x.Company).Include(x => x.Investigators).Include(x => x.InvestigationStages)
                       .Where(x => x.Investigators.Any(x => x.LocalUserId == currentUserId));

        }

        public async Task<AdministrativeInspection> GetById(int id)
        {
            {
                var complain = await _db.AdministrativeInspection.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.InvestigationStages).FirstOrDefaultAsync(x => x.AdministrativeInspectionId == id);
                return complain;


            }
        }

    }
}
