using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;

namespace UTP_Web_API.Models.Dto.InvestigationDto
{
    public class GetInvestigationsDto
    {
        public GetInvestigationsDto()
        {
        }
        public int InvestigationId { get; set; }
        /// <summary>
        /// Imones, kurios atzvilgiu vykdomas tyrimas
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// tyrimo etapu sarasas, su etapo data ir aprasymu
        /// </summary>
        public ICollection<GetInvestigationStagesDto>? InvestigationStage { get; set; }
        public string InvestigationStarted { get; set; }
        public string? InvestigationEnded { get; set; }
        public string? Conclusion { get; set; }
        public ICollection<GetInvestigatorNameDto> Investigator  { get; set; }
        public int Penalty  { get; set; }
    }
}
