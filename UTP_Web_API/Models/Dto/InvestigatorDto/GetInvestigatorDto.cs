namespace UTP_Web_API.Models.Dto.InvestigatorDto
{
    public class GetInvestigatorDto
    {
        public GetInvestigatorDto()
        {
        }

        public GetInvestigatorDto(Investigator investigator)
        {
            Vardas = investigator.LocalUser?.FirstName;
            Pavarde = investigator.LocalUser?.LastName;
            TelefonoNumeris = (investigator.LocalUser?.PhoneNumber);
            ElPastas = investigator.LocalUser?.Email;
            PazymejimoNumeris = investigator?.CertificationId;
            KabinetoNumeris = investigator?.CabinetNumber;
            DarboVietosAdresas = investigator?.WorkAdress;
        }

        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public long? TelefonoNumeris { get; set; }
        public string ElPastas { get; set; }
        public string PazymejimoNumeris { get; set; }
        public string KabinetoNumeris { get; set; }
        public string DarboVietosAdresas { get; set; }

    }
}
