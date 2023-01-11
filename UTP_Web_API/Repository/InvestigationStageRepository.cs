using UTP_Web_API.Database;
using UTP_Web_API.Models;
using UTP_Web_API.Repository.IRepository;

namespace UTP_Web_API.Repository
{
    public class InvestigationStageRepository : Repository<InvestigationStage>, IInvestigationStagesRepository
    {
        public InvestigationStageRepository(UtpContext db) : base(db)
        {
        }



    }
}
