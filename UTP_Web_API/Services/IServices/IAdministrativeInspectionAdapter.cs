using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.AdminInspection;

namespace UTP_Web_API.Services.IServices
{
    public interface IAdministrativeInspectionAdapter
    {
        GetAdministrativeInspectionsDto Bind(AdministrativeInspection inspection);
        GetOneAdministrativeInspectionDto BindOneInspection(AdministrativeInspection inspection);
    }
}
