namespace UTP_Web_API.Models.Dto
{
    public class GetInvestigationStagesDto
    {
        public GetInvestigationStagesDto(InvestigationStage stage)
        {
            Etapas = stage.Stage;
            EtapoPradzia = stage.TimeStamp;
        }

        public string Etapas { get; set; }
        public DateTime EtapoPradzia { get; set; }
    }
}
