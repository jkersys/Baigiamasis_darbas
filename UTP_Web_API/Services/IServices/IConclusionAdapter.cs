using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ConclusionDto;

namespace UTP_Web_API.Services.IServices
{
    public interface IConclusionAdapter 
    {
        GetConclusionDto Bind(Conclusion conclusion);
        Conclusion Bind(string isvada);
    }
}
