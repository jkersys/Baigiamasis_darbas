using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IInvestigatorComplainsRepository : IRepository<Complain>
    {
        Task<Complain> GetComplainById(int id);
    }
}
