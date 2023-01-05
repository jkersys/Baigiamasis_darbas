using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;

namespace UTP_Web_API.Services
{
    public interface IAdapter
    {
        Investigator Bind(LocalUser localUser, CreateInvestigatorDto createInvestigator);
    }
}
