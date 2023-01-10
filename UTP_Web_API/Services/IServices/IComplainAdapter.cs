using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto;

namespace UTP_Web_API.Services.IServices
{
    public interface IComplainAdapter
    {
        Complain Bind(CreateComplainDto complain, LocalUser user);
        GetComplainDto Bind(Complain complain);
    }
}
