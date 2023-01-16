using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class ComplainRepository : Repository<Complain>, IComplainRepository
    {
        private readonly UtpContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComplainRepository(UtpContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Complain>> All()
        {
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (currentUserRole == "Admin" || currentUserRole == "Director")
            {
                var complains = await _db.Complain.Include(x => x.Investigator.LocalUser).Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Stages).ToListAsync();
                return complains;
            }
            if (currentUserRole == "Investigator")
            {
                var complains = await _db.Complain.Include(x => x.Investigator.LocalUser).Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Stages).Where(x => x.Investigator.LocalUserId == currentUserId).ToListAsync();
                return complains;
            }
            if (currentUserRole == "Customer")
            {
                var complains = await _db.Complain.Include(x => x.Investigator.LocalUser).Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Stages).Where(x => x.LocalUser.Id == currentUserId).ToListAsync();
                return complains;
            }
            else return null;
        }

        public async Task<Complain> GetById(int id)
        {
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.Identity.Name);
            var currentUserRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

            if (currentUserRole == "Customer" || currentUserRole == "Investigator")
            {
                var complain = await _db.Complain.Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Investigator.LocalUser).Include(x => x.Stages).Where(x => x.LocalUser.Id == currentUserId).FirstOrDefaultAsync(x => x.ComplainId == id);
                return complain;
            }

            if (currentUserRole == "Admin" || currentUserRole == "Director")
            {
                var complain = await _db.Complain.Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Investigator.LocalUser).Include(x => x.Stages).FirstOrDefaultAsync(x => x.ComplainId == id);
                return complain;
            }
            else return null;
        }
               
    }
    }

