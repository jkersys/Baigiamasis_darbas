using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.InvestigationDto;

namespace UTP_Web_API.Services.IServices
{
    public interface IInvestigationAdapter
    {
        GetInvestigationsDto Bind(Investigation investigation);
        GetOneInvestigationDto BindForOneInvestigation(Investigation investigation);
    }
}
