using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class InvestigationRepository : Repository<Investigation>, IInvestigationRepository
    {
        private readonly UtpContext _db;
        public InvestigationRepository(UtpContext db) : base(db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Investigation>> All()
        {
            var investigations = _db.Investigation.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.Stages);
            return await investigations.ToListAsync();
            //return complain;
        }
        public async Task<Investigation> GetById(int id)
        {
            var investigation = await _db.Investigation.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.Stages).FirstOrDefaultAsync(x => x.InvestigationId == id);
            return investigation;
        }
    }
}
