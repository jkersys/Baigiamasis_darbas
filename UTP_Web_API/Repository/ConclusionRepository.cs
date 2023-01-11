using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class ConclusionRepository : Repository<Conclusion>, IConclusionRepository
    {       
        public ConclusionRepository(UtpContext db) : base(db)
        {           
        }
    }
}
