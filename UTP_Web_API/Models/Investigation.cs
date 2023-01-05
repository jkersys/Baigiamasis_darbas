namespace UTP_Web_API.Models
{
    public class Investigation
    {
        public int InvestigationId { get; set; }
        public int InvestigatorId { get; set; }
        public int CompanyId { get; set; }
        public string LegalBase { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Conclusion { get; set; }
        public int Penalty { get; set; }

    }
}
