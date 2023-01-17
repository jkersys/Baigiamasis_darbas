using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class InvestigationRepository : Repository<Investigation>, IInvestigationRepository
    {
        private readonly UtpContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvestigationRepository(UtpContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Investigation>> All()
        {
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (currentUserRole == "Admin" || currentUserRole == "Director")
            {
                return await _db.Investigation.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.Stages).ToListAsync();

            }
            else
                return _db.Investigation.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Stages)
                       .Include(x => x.Company)
                       .Where(x => x.Investigators.Any(x => x.LocalUserId == currentUserId));
        }

        public async Task<Investigation> GetById(int id)
        {
            var investigation = await _db.Investigation.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.Stages).FirstOrDefaultAsync(x => x.InvestigationId == id);
            return investigation;
        }
    }
}
