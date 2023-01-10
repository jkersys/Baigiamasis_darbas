using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;

namespace UTP_Web_API.Services
{
    public interface IInvestigatorAdapter
    {
        Investigator Bind(CreateInvestigatorDto createInvestigator);
        GetInvestigatorDto Bind(Investigator investigator);
    }
}
