using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.AdminInspection;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Services
{
    public class AdministrativeInspectionAdapter : IAdministrativeInspectionAdapter
    {
        public GetAdministrativeInspectionsDto Bind(AdministrativeInspection inspection)
        {
            return new GetAdministrativeInspectionsDto()
            {
                Id = inspection.AdministrativeInspectionId,
                StartDate = inspection.StartDate.ToString("yyyy-MM-dd"),
                EndDate = inspection.EndDate?.ToString("yyyy-MM-dd"),
                InvestigationStages = inspection.InvestigationStages.Select(i => new GetInvestigationStagesDto(i)).ToList(),
                Company = inspection.Company.CompanyName,
                Investigators = inspection.Investigators.Select(i => new GetInvestigatorNameDto(i)).ToList(),
                Conclusion = inspection.Conclusion?.Decision,
            };
        }

        public GetOneAdministrativeInspectionDto BindOneInspection(AdministrativeInspection inspection)
        {
            return new GetOneAdministrativeInspectionDto()
            {
                StartDate = inspection.StartDate.ToString("yyyy-MM-dd"),
                EndDate = inspection.EndDate?.ToString("yyyy-MM-dd"),
                InvestigationStages = inspection.InvestigationStages.Select(st => new GetInvestigationStagesDto(st)).ToList(),
                Company = new Models.Dto.CompanyDto.GetCompanyDto(inspection.Company),
                Investigators = inspection.Investigators.Select(i => new GetInvestigatorDto(i)).ToList(),
                Conclusion = inspection.Conclusion?.Decision,

            };
        }
    }
}
