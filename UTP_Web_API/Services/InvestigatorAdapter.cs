using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigatorDto;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Services
{
    public class InvestigatorAdapter : IInvestigatorAdapter
    {
        public Investigator Bind(CreateInvestigatorDto createInvestigator, LocalUser user)
        {
            return new Investigator()
            {
                
                LocalUser = user,                
                CertificationId = createInvestigator.CertificateNumber,
                CabinetNumber = createInvestigator.CabinetNumber,
                WorkAdress = createInvestigator.WorkplaceAdress
                
            };
        }

        public GetInvestigatorDto Bind(Investigator investigator)
        {
            return new GetInvestigatorDto()
            {
               Name = investigator.LocalUser.FirstName,
               Lastname = investigator.LocalUser.LastName,
               PhoneNumber = investigator.LocalUser.PhoneNumber,
               Email = investigator.LocalUser.Email,
               CertificateNumber = investigator.CertificationId,
               CabinetNumber = investigator.CabinetNumber,
               WorkplaceAdress = investigator.WorkAdress,
            };
        }
        public InvestigatorResponse BindForFrontEndResponse(Investigator investigator)
        {
            return new InvestigatorResponse()
            {
                id = investigator.InvestigatorId,
                NameAndLastName = investigator.LocalUser.FirstName + " " + investigator.LocalUser.LastName

            };
        }
       

    }
}
