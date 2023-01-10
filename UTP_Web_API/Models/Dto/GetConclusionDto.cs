namespace UTP_Web_API.Models.Dto
{
    public class GetConclusionDto
    {
        public GetConclusionDto()
        {
        }

        public GetConclusionDto(Conclusion? conclusion)
        {
            Isvada = conclusion.Decision;
        }

        public string? Isvada { get; set; }
    }
}

