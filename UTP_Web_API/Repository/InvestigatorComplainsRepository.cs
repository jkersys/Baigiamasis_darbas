using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class InvestigatorComplainsRepository : Repository<Complain>, IInvestigatorComplainsRepository
    {
        private readonly UtpContext _db;
        public InvestigatorComplainsRepository(UtpContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Complain> GetComplainById(int id)
        {
            var complain = await _db.Complain.Include(x => x.Conclusion).FirstOrDefaultAsync(x => x.ComplainId == id);
            return complain;
        }
    }
}
