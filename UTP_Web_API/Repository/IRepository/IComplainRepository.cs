using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IComplainRepository : IRepository<Complain>
    {
        Task<IEnumerable<Complain>> All();
        Task<Complain> GetById(int id);
    }
}
