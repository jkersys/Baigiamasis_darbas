using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.CompanyDto;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Services
{
    public class CompanyAdapter : ICompanyAdapter
    {     

        public GetCompanyDto Bind(Company company)
        {
            return new GetCompanyDto()
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyRegistrationNumber = company.CompanyRegistrationNumber,
                CompanyAdress = company.CompanyAdress,
                CompanyEmail = company.CompanyEmail,
                CompanyPhone = company.CompanyPhone
            };
        }

        public Company Bind(CreateCompanyDto updateCompany)
        {
            return new Company()
            {
                CompanyName = updateCompany.CompanyName,
                CompanyRegistrationNumber = updateCompany.CompanyRegistrationNumber,
                CompanyAdress = updateCompany.CompanyAdress,
                CompanyEmail = updateCompany.CompanyEmail,
                CompanyPhone = updateCompany.CompanyPhone
            };
        }
    }
}
