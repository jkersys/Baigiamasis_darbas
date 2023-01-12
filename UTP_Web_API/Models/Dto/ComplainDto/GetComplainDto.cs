﻿using UTP_Web_API.Models.Dto.InvestigationStageDto;

namespace UTP_Web_API.Models.Dto.ComplainDto
{
    public class GetComplainDto
    {
        public GetComplainDto()
        {        
        }

        public string Pareiškėjas { get; set; }
        public long TelefonoNumeris { get; set; }
        public string SituacijosAprasymas { get; set; }
        public string DuomenysApieSkundziamaImone { get; set; }
        public ICollection<GetInvestigationStagesDto>? Etapas { get; set; }
        public DateTime SkundasPaduodas { get; set; }
        public DateTime? SkundrasIsnagrinetas { get; set; }
        public string? Isvada { get; set; }
        public string Tyrejas { get; set; }
        public long? TyrejoTelefonoNumeris { get; set; }
      

    }
}
