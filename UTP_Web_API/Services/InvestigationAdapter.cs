using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigationDto;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Models.Dto.InvestigatorDto;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Services
{
    public class InvestigationAdapter : IInvestigationAdapter
    {
        public Investigation Bind(CreateInvestigationDto investigation)
        {
            return new Investigation()
            {
                CompanyId = investigation.CompanyId,
                LegalBase = investigation.LegalBase,
                StartDate = DateTime.Now,
            };
        }
        public GetInvestigationsDto Bind(Investigation investigation)
        {
            return new GetInvestigationsDto()
            {
                InvestigationId = investigation.CompanyId,
                Company = investigation.Company.CompanyName,
                InvestigationStage = investigation.Stages.Select(st => new GetInvestigationStagesDto(st)).ToList(),
                InvestigationStarted = investigation.StartDate.ToString("yyyy-MM-dd"),
                InvestigationEnded = investigation.EndDate?.ToString("yyyy-MM-dd"),
                Investigator = investigation.Investigators.Select(i => new GetInvestigatorNameDto(i)).ToList(),
                Conclusion = investigation.Conclusion?.Decision,
                Penalty = investigation.Penalty,
            };
        }
        public GetOneInvestigationDto BindForOneInvestigation(Investigation investigation)
        {
            return new GetOneInvestigationDto()
            {
                InvestigationId = investigation.InvestigationId,
                Company = new Models.Dto.CompanyDto.GetCompanyDto(investigation.Company),
                InvestigationStage = investigation.Stages.Select(st => new GetInvestigationStagesDto(st)).ToList(),
                InvestigationStarted = investigation.StartDate.ToString("yyyy-MM-dd"),
                InvestigationEnded = investigation.EndDate?.ToString("yyyy-MM-dd"),
                Investigator = investigation.Investigators.Select(i => new GetInvestigatorDto(i)).ToList(),
                Conclusion = investigation.Conclusion?.Decision,
                Penalty = investigation.Penalty,
            };
        }
    }
}
            
     
   

    

