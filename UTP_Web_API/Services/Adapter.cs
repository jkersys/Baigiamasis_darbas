using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;

namespace UTP_Web_API.Services
{
    public class Adapter : IAdapter
    {
        public Investigator Bind(LocalUser localUser, CreateInvestigatorDto createInvestigator)
        {
            return new Investigator()
            {
                LocalUserId = localUser.Id,
                PhoneNumber = createInvestigator.TelefonoNumeris,
                CertificationId = createInvestigator.PazymejimoNumeris,
                CabinetNumber = createInvestigator.KabinetoNumeris,
                WorkAdress = createInvestigator.DarboAdresas
            };
        }
    }
}
