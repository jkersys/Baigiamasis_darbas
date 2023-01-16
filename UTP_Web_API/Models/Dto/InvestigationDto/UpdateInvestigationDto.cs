namespace UTP_Web_API.Models.Dto.InvestigationDto
{
    public class UpdateInvestigationDto
    {
        public int? CompanyId { get; set; }
        /// <summary>
        /// Teisinis tyrimo pagrindas
        /// </summary>
        public string? LegalBase { get; set; }
        public string? InvestigationStage { get; set; }
        public int? InvestigatorId { get; set; }
    }
}
