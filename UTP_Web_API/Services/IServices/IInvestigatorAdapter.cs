using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigatorDto;

namespace UTP_Web_API.Services.IServices
{
    public interface IInvestigatorAdapter
    {
        Investigator Bind(CreateInvestigatorDto createInvestigator, LocalUser user);
        GetInvestigatorDto Bind(Investigator investigator);
        InvestigatorResponse BindForFrontEndResponse(Investigator investigator);
    }
}
