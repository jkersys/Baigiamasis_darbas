namespace UTP_Web_API.Models.Dto.ConclusionDto
{
    public class ConclusonResponseForFrontEnd
    {
        public ConclusonResponseForFrontEnd(Conclusion conclusion)
        {
            Id = conclusion.ConclusionId;
            Conclusion = conclusion.Decision;
        }

        public int Id { get; set; }
        /// <summary>
        /// isvados tekstas
        /// </summary>
        public string Conclusion { get; set; }
    }
}
