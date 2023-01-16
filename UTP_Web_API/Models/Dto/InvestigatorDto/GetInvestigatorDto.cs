namespace UTP_Web_API.Models.Dto.InvestigatorDto
{
    public class GetInvestigatorDto
    {
        public GetInvestigatorDto()
        {
        }

        public GetInvestigatorDto(Investigator investigator)
        {
            Name = investigator.LocalUser?.FirstName;
            Lastname = investigator.LocalUser?.LastName;
            PhoneNumber = (investigator.LocalUser?.PhoneNumber);
            Email = investigator.LocalUser?.Email;
            CertificateNumber = investigator?.CertificationId;
            CabinetNumber = investigator?.CabinetNumber;
            WorkplaceAdress = investigator?.WorkAdress;
        }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public long? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CertificateNumber { get; set; }
        public string CabinetNumber { get; set; }
        public string WorkplaceAdress { get; set; }

    }
}
