using UTP_Web_API.Models;
using UTP_Web_API.Models.Dto.ConclusionDto;
using UTP_Web_API.Services.IServices;

namespace UTP_Web_API.Services
{
    public class ConclusionAdapter : IConclusionAdapter
    {
        public GetConclusionDto Bind(Conclusion conclusion)
        {
            return new GetConclusionDto()
            {
                Conclusion = conclusion?.Decision
            };
        }
        
        public Conclusion Bind(string isvada)
        {
            return new Conclusion()
            {
                Decision = isvada
            };
        }

    }
}
