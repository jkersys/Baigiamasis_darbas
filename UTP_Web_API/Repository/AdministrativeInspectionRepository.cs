using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class AdministrativeInspectionRepository : Repository<AdministrativeInspection>, IAdministrativeInspectionRepository
    {
    private readonly UtpContext _db;
        public AdministrativeInspectionRepository(UtpContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AdministrativeInspection>> All()
        {
            var complain = await _db.AdministrativeInspection.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.InvestigationStages).ToListAsync();
            return complain;
        }

        public async Task<AdministrativeInspection> GetById(int id)
        {
            var complain = await _db.AdministrativeInspection.Include(x => x.Investigators).ThenInclude(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Company).Include(x => x.InvestigationStages).FirstOrDefaultAsync(x => x.AdministrativeInspectionId == id);
            return complain;
        }
    }
   
}
