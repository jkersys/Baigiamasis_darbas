namespace UTP_Web_API.Models.Dto
{
    public class GetComplainDto
    {
        public GetComplainDto()
        {
        }

        public GetComplainDto(Complain complain)
        {
            SituacijosAprasymas = complain.Description;
            DuomenysApieSkundziamaImone = complain.CompanyInformation;
            SkundasPaduodas = complain.StartDate;
            SkundrasIsnagrinetas = complain.EndDate;
            Isvada = complain.Conclusion?.Decision;
            //InvestigatorDto = new GetInvestigatorDto();
            //GetInvestigationStagesDtos = complain.Stages
           //.Select(s => new GetInvestigationStagesDto(s)) //permapinam recipeItem i GetRecipeItemDto (gaunam nauja objekta, pasiimam tik tuos parametrus, kuriu reikia frontendo vaizdavimui)
          //.ToList();

        }

        public LocalUser Pareiškėjas { get; set; }
        public string SituacijosAprasymas { get; set; }
        public string DuomenysApieSkundziamaImone { get; set; }
        public DateTime SkundasPaduodas { get; set; }
        public DateTime? SkundrasIsnagrinetas { get; set; }
        public string? Isvada { get; set; }
       // public GetConclusionDto? Isvada { get; set; } = new GetConclusionDto();
        // public List<GetInvestigationStagesDto>? GetInvestigationStagesDtos { get; set; } = new List<GetInvestigationStagesDto>();
        // public GetInvestigatorDto? InvestigatorDto = new GetInvestigatorDto();

    }
}
