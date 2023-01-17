using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class ConclusionRepository : Repository<Conclusion>, IConclusionRepository
    {
        private readonly UtpContext _db;
        public ConclusionRepository(UtpContext db) : base(db)
        {
                _db = db;
        }
        public async Task<Conclusion> GetConclusionById(int id)
        {
            var conclusion = await _db.Conclusion.Include(x => x.Complains).Include(x => x.AdministrativeInspections).Include(x => x.Investigations).FirstOrDefaultAsync(x => x.ConclusionId == id);
            return conclusion;
        }
    }
}
