﻿using Microsoft.EntityFrameworkCore;
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
            var complains = await _db.Complain.Include(x => x.Investigator.LocalUser).Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Stages).ToListAsync();
            return complains;
        }

        public async Task<Complain> GetById(int id)
        {
            var complain = await _db.Complain.Include(x => x.LocalUser).Include(x => x.Conclusion).Include(x => x.Investigator.LocalUser).Include(x => x.Stages).FirstOrDefaultAsync(x => x.ComplainId == id);
            return complain;
        }
               
    }
    }

