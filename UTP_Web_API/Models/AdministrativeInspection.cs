namespace UTP_Web_API.Models
{
    public class AdministrativeInspection
    {
        public int AdministrativeInspectionId { get; set; }
        public int InvestigatorId { get; set; }
        public string CompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public IEnumerable<InvestigationStage> InvestigationStages { get; set; }
        public Company Company { get; set; }
        public IEnumerable<Investigator> Investigators { get; set; }
        public string ConclusionId { get; set; }


    }
}
