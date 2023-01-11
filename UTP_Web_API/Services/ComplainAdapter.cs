using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;
using UTP_Web_API.Models.Dto.ComplainDto;
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
                Pareiškėjas = complain.LocalUser.FirstName + " " + complain.LocalUser.LastName,
                TelefonoNumeris = complain.LocalUser.PhoneNumber,
                SituacijosAprasymas = complain.Description,
                DuomenysApieSkundziamaImone = complain.CompanyInformation,
                SkundasPaduodas = complain.StartDate,
                SkundrasIsnagrinetas = complain.EndDate,
                Etapas = complain.Stages.Select(st => new GetInvestigationStagesDto(st)).ToList(),
                Tyrejas = complain.Investigator?.LocalUser.FirstName + " " + complain.Investigator?.LocalUser.LastName,
                TyrejoTelefonoNumeris = complain.Investigator?.LocalUser.PhoneNumber,
                Isvada = complain.Conclusion?.Decision,
               
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

