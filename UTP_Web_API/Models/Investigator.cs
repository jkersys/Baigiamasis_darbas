namespace UTP_Web_API.Models
{
    public class Investigator
    {
        public int InvestigatorId { get; set; }
        public string InvestigatorName { get; set; }
        public string InvestigatorLastname { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string CertificationId { get; set; }
        public string CabinetNumber { get; set; }
        public string WorkAdress { get; set; }
        public LocalUser LocalUser { get; set; }
        public int LocalUserId { get; set; }





    }
}
