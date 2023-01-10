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
        public ICollection<Complain> Complains { get; set; }
        public ICollection<AdministrativeInspection> AdministrativeInspections { get; set; }
        public ICollection<Investigation> Investigations { get; set; }
    }
}
