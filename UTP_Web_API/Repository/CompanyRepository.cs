using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(UtpContext db) : base(db)
        {
        }
    }
}
