using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;

namespace UTP_Web_API.Models.Dto.InvestigationDto
{
    public class GetInvestigationsDto
    {
        public GetInvestigationsDto()
        {
        }

        public string Company { get; set; }
        public ICollection<GetInvestigationStagesDto>? InvestigationStage { get; set; }
        public string InvestigationStarted { get; set; }
        public string? InvestigationEnded { get; set; }
        public string? Conclusion { get; set; }
        public ICollection<GetInvestigatorNameDto> Investigator  { get; set; }
        public int Penalty  { get; set; }
    }
}
