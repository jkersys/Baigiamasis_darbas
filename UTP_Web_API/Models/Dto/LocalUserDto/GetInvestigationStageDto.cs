namespace UTP_Web_API.Models.Dto.LocalUserDto
{
    public class GetInvestigationStageDto
    {
        public GetInvestigationStageDto(InvestigationStage investigationStage)
        {
            Stage = investigationStage.Stage;
            TimeStamp = investigationStage.TimeStamp;
        }

        public string Stage { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
