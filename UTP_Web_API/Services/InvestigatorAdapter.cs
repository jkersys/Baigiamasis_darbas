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
                CertificationId = createInvestigator.PazymejimoNumeris,
                CabinetNumber = createInvestigator.KabinetoNumeris,
                WorkAdress = createInvestigator.DarboAdresas
            };
        }

        public GetInvestigatorDto Bind(Investigator investigator)
        {
            return new GetInvestigatorDto()
            {
               Vardas = investigator.LocalUser.FirstName,
               Pavarde = investigator.LocalUser.LastName,
               TelefonoNumeris = investigator.LocalUser.PhoneNumber,
               ElPastas = investigator.LocalUser.Email,
               PazymejimoNumeris = investigator.CertificationId,
               KabinetoNumeris = investigator.CabinetNumber,
               DarboVietosAdresas = investigator.WorkAdress,
            };
        }
    }
}
