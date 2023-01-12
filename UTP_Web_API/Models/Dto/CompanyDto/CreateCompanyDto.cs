using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models.Dto.CompanyDto
{
    public class CreateCompanyDto
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public int CompanyRegistrationNumber { get; set; }
        [Required]
        public string CompanyAdress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
    }
}
