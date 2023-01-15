using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;
using UTP_Web_API.Models.Dto.ComplainDto;
using UTP_Web_API.Models.Dto.InvestigationStageDto;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Services
{
    public class ComplainAdapter : IComplainAdapter
    {
        public Complain Bind(CreateComplainDto complain, LocalUser user)
        {
            return new Complain
            {
                Description = complain.Description,
                CompanyInformation = complain.CompanyInformation,
                StartDate = DateTime.Now,
                LocalUser = user
            };
        }

        public GetComplainDto Bind(Complain complain)
        {
            return new GetComplainDto
            {
                ComplainId = complain.ComplainId,
                Complainant = complain.LocalUser.FirstName + " " + complain.LocalUser.LastName,
                ComplainantPhoneNumer = complain.LocalUser.PhoneNumber,
                ComplaintDescription = complain.Description,
                CompanyDetails = complain.CompanyInformation,
                ComplainStartDate = complain.StartDate.ToString("yyyy-MM-dd"),
                ComplainEndDate = complain.EndDate?.ToString("yyyy-MM-dd"),
                ComplainStage = complain.Stages.Select(st => new GetInvestigationStagesDto(st)).ToList(),
                Investigator = complain.Investigator?.LocalUser.FirstName + " " + complain.Investigator?.LocalUser.LastName,
                InvestigatorPhoneNumber = complain.Investigator?.LocalUser.PhoneNumber,
                Conclusion = complain.Conclusion?.Decision,
               
            };

        }

        public Complain Bind(Investigator investigator, Complain complain, string stage)
        {
            var curentStage = new InvestigationStage
            {
                Stage = stage,
                TimeStamp = DateTime.Now
            };
            return null;
          
        }

    }
}

