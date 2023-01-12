using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.CompanyDto;

namespace UTP_Web_API.Services.IServices
{
    public interface ICompanyAdapter
    {
        GetCompanyDto Bind(Company company);
        Company Bind(CreateCompanyDto updateCompany);
    }
}
