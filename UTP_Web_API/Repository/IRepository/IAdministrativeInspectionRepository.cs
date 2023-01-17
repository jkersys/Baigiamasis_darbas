using UTP_Web_API.Models;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IAdministrativeInspectionRepository : IRepository<AdministrativeInspection>
    {
        Task<IEnumerable<AdministrativeInspection>> LoggedUserAdministrativeInspecitons();
        Task<AdministrativeInspection> GetById(int id);
    }
}
