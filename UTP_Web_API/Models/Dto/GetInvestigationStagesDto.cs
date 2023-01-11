namespace UTP_Web_API.Models.Dto
{
    public class GetInvestigationStagesDto
    {
        public GetInvestigationStagesDto(InvestigationStage stage)
        {
            Etapas = stage.Stage;
            EtapoPradzia = stage.TimeStamp.ToString("yyyy-MM-dd");
        }

        public string Etapas { get; set; }
        public string EtapoPradzia { get; set; } 
    }
}
