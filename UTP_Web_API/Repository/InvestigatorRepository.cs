using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class InvestigatorRepository : Repository<Investigator>, IInvestigatorRepository
    {
        private readonly UtpContext _db;
        public InvestigatorRepository(UtpContext db) : base(db)
        {
            _db = db;
        }
            public async Task<IEnumerable<Investigator>> All()
            {
                var investigators = await _db.Investigator.Include(x => x.LocalUser).ToListAsync();
                return investigators;
            }

            public async Task<Investigator> GetById(int id)
            {
                var investigators = await _db.Investigator.Include(x => x.LocalUser).Include(x => x.Complains).Include(x => x.AdministrativeInspections).Include(x => x.Investigations).FirstOrDefaultAsync(x => x.InvestigatorId == id);
                return investigators;
            }
        }
    }

