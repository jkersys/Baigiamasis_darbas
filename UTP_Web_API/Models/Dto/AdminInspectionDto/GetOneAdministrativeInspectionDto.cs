using UTP_Web_API.Models.Dto.CompanyDto;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;

namespace UTP_Web_API.Models.Dto.AdminInspection
{
    public class GetOneAdministrativeInspectionDto
    {
     
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public ICollection<GetInvestigationStagesDto> InvestigationStages { get; set; }
        public GetCompanyDto Company { get; set; }
        public ICollection<GetInvestigatorDto> Investigators { get; set; }
        public string? Conclusion { get; set; }
    }
}
