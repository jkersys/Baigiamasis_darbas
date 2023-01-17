using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> GetCompanyById(int id);
    }
}
