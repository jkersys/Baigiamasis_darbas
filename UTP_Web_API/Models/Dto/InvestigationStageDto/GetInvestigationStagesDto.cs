namespace UTP_Web_API.Models.Dto.InvestigationStageDto
{
    public class GetInvestigationStagesDto
    {
        public GetInvestigationStagesDto(InvestigationStage stage)
        {
            Stage = stage.Stage + " " +  stage.TimeStamp.ToString("yyyy-MM-dd");
            //TimeStamp = stage.TimeStamp.ToString("yyyy-MM-dd");
        }

        public string Stage { get; set; }
       // public string TimeStamp { get; set; }
    }
}
