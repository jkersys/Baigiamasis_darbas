namespace UTP_Web_API.Models
{
    public class Complain
    {
        public int ComplainId { get; set; }
        public int InvestigatorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Conclusion { get; set; }
    }
}
