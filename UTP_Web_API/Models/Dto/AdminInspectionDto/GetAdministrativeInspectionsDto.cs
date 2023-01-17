using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;

namespace UTP_Web_API.Models.Dto.AdminInspection
{
    public class GetAdministrativeInspectionsDto
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
        public ICollection<GetInvestigationStagesDto> InvestigationStages { get; set; }
        public string Company { get; set; }
        public ICollection<GetInvestigatorNameDto> Investigators { get; set; }
        public string? Conclusion { get; set; }
    }
}
