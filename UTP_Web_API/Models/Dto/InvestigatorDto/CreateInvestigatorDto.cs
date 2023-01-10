using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models.Dto.InvestigatorDto
{
    public class CreateInvestigatorDto
    {
        [Required]
        public string PazymejimoNumeris { get; set; }
        [Required]
        public string KabinetoNumeris { get; set; }
        [Required]
        public string DarboAdresas { get; set; }
        [Required]
        public string VartotojoElPastas { get; set; }
    }
}
