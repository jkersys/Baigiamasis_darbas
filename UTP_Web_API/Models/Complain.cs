namespace UTP_Web_API.Models
{
    public class Complain
    {
        public int ComplainId { get; set; }
        public int InvestigatorId { get; set; }
        public int LocalUserId { get; set; }
        public string Description { get; set; }
        public string CompanyInformation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Conclusion Conclusion { get; set; }
        public LocalUser LocalUser { get; set; }
        
        public string Conclusion { get; set; }
        public IEnumerable<InvestigationStage> Stages { get; set; }
    }
}
