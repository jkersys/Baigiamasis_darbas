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
        public DateTime TimeStamp { get; set; }
        public IEnumerable<Complain> Complains { get; set; }
        public IEnumerable<AdministrativeInspection> AdministrativeInspections { get; set; }
        public IEnumerable<Investigation> Investigations { get; set; }
    }
}
