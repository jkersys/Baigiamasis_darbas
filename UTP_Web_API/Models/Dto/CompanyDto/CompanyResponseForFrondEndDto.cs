namespace UTP_Web_API.Models.Dto.CompanyDto
{
    public class CompanyResponseForFrondEndDto
    {
        public CompanyResponseForFrondEndDto(Company company)
        {
            CompanyId = company.CompanyId;
            CompanyName = company.CompanyName;
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
