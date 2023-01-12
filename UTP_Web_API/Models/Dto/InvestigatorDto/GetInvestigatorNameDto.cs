namespace UTP_Web_API.Models.Dto.InvestigatorDto
{
    public class GetInvestigatorNameDto
    {
        public GetInvestigatorNameDto(Investigator investigator)
        {
            NameAndLastName = investigator.LocalUser?.FirstName + " " + investigator.LocalUser?.LastName;
        }
        public string NameAndLastName { get; set; }
    }
}
