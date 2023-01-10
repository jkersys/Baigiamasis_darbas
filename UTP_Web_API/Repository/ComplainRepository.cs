using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class ComplainRepository : Repository<Complain>, IComplainRepository
    {
        private readonly UtpContext _db;

        public ComplainRepository(UtpContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Complain>> All()
        {
            var complains = _db.Complain.Include(x => x.Conclusion).Include(x => x.Stages).Include(x => x.Investigator).ToList();
            return complains;
        }

        public Complain GetById(int id)
        {
            var complain = _db.Complain.Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Investigator).First(x => x.ComplainId == id);
           // var complains = _db.Complain.First(x => x.ComplainId == id);
            return complain;
        }

        //public override async Task<List<Complain>> GetAllAsync(Expression<Func<Complain, bool>>? filter = null)
        //{
        //    IQueryable<Complain> query = _dbSet;
        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.ToListAsync();
    }
    }

