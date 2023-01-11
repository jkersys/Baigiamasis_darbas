using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models
{
    public class InvestigationStage
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestigationStageId { get; set; }
        public string Stage { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public Complain Complain { get; set; }

        public int? ComplainId { get; set; }
        public int? AdministrativeInspectionId { get; set; }
        public int? InvestigationId { get; set; }
    }
}
