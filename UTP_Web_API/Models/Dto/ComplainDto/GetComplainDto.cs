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
        /// <summary>
        /// grazinamas etapu sarasas, kuris yra is esmes kaip istorija ir nurodo etapo data ir aprasyma 
        /// </summary>
        public ICollection<GetInvestigationStagesDto>? ComplainStage { get; set; }
        /// <summary>
        /// skundo pradzioj data yra data, kuomet skundas kuriamas
        /// </summary>
        public string ComplainStartDate { get; set; }
        public string? ComplainEndDate { get; set; }
        /// <summary>
        /// Ikeliama galutine isvada, koks sprendimas priimtas
        /// </summary>
        public string? Conclusion { get; set; }
        /// <summary>
        /// Grazinamas tyrejo vardas ir pavarde
        /// </summary>
        public string Investigator { get; set; }
        /// <summary>
        /// atiduodamas tyrejo tel. numeris
        /// </summary>
        public long? InvestigatorPhoneNumber { get; set; }
      

    }
}
