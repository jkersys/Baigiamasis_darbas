namespace UTP_Web_API.Models.Dto.ComplainDto
{
    public class CreateComplainDto
    {
        /// <summary>
        /// nurodoma skundo aplinkybes
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Nurodoma visa zinoma informacija apie skundziama imone
        /// </summary>
        public string CompanyInformation { get; set; }
    }
}
