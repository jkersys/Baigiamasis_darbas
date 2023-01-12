using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IInvestigationRepository : IRepository<Investigation>
    {
        Task<Investigation> GetById(int id);
        Task<IEnumerable<Investigation>> All();
    }
}
