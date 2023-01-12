namespace UTP_Web_API.Models.Dto.CompanyDto
{
    public class GetCompanyDto
    {
        public GetCompanyDto()
        {
        }

        public GetCompanyDto(Company company)
        {
            CompanyName = company.CompanyName;
            CompanyRegistrationNumber = company.CompanyRegistrationNumber;
            CompanyAdress = company.CompanyAdress;
            CompanyEmail = company.CompanyEmail;
            CompanyPhone = company.CompanyPhone;
        }

        public string CompanyName { get; set; }
        public int CompanyRegistrationNumber { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
    }
}
