using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models.Dto.InvestigationDto
{
    public class CreateInvestigationDto
    {
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string LegalBase { get; set; }
        [Required]
        public string InvestigationStage { get; set; }
        [Required]
        public int InvestigatorId { get; set; }
    }
}
