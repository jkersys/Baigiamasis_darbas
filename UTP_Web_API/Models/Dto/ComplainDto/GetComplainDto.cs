using UTP_Web_API.Models.Dto.InvestigationStageDto;

namespace UTP_Web_API.Models.Dto.ComplainDto
{
    public class GetComplainDto
    {
        public GetComplainDto()
        {        
        }
        public int ComplainId { get; set; }
        public string Complainant { get; set; }
        public long ComplainantPhoneNumer { get; set; }
        public string ComplaintDescription { get; set; }
        public string CompanyDetails { get; set; }
        public ICollection<GetInvestigationStagesDto>? ComplainStage { get; set; }
        public string ComplainStartDate { get; set; }
        public string? ComplainEndDate { get; set; }
        public string? Conclusion { get; set; }
        public string Investigator { get; set; }
        public long? InvestigatorPhoneNumber { get; set; }
      

    }
}
