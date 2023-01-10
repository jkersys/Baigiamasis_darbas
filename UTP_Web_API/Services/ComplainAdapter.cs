using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;
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
                Pareiškėjas = complain.LocalUser,
                SituacijosAprasymas = complain.Description,
                DuomenysApieSkundziamaImone = complain.CompanyInformation,
                SkundasPaduodas = complain.StartDate,
                SkundrasIsnagrinetas = complain.EndDate,
               // Isvada = new GetConclusionDto()

                Isvada = complain.Conclusion?.Decision,
               
            };



        }
    }
}

