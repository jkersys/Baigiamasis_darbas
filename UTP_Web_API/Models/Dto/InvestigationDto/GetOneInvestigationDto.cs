using UTP_Web_API.Models.Dto.CompanyDto;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;

namespace UTP_Web_API.Models.Dto.InvestigationDto
{
    public class GetOneInvestigationDto
    {
        public int InvestigationId { get; set; }
        /// <summary>
        /// grazinama visa imones, kurios atzvilgiu vykdomas tyrimas informacija
        /// </summary>
        public GetCompanyDto Company { get; set; }
        /// <summary>
        /// tyrimo etapu sarasas, su etapo data ir aprasymu
        /// </summary>
        public ICollection<GetInvestigationStagesDto>? InvestigationStage { get; set; }
        public string InvestigationStarted { get; set; }
        public string? InvestigationEnded { get; set; }
        public string? Conclusion { get; set; }
        public ICollection<GetInvestigatorDto> Investigator { get; set; }
        public int Penalty { get; set; }
    }
}
