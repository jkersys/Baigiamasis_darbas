using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IConclusionRepository : IRepository<Conclusion>
    {
        Task<Conclusion> GetConclusionById(int id);
    }
}
