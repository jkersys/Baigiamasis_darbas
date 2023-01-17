using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly UtpContext _db;

        public CompanyRepository(UtpContext db) : base(db)
        {
            _db = db;

        }
        public async Task<Company> GetCompanyById(int id)
        {
            var company = await _db.Company.Include(x => x.AdministrativeInspections).Include(x => x.Investigations).FirstOrDefaultAsync(x => x.CompanyId == id);
            return company;
        }
    }
}
