using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IInvestigatorRepository : IRepository<Investigator>
    {
        Task<IEnumerable<Investigator>> All();
        Task<Investigator> GetById(int id);
    }
}
