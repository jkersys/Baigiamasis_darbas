using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTP_Web_API.Models
{
    public class AdministrativeInspection
    {
        public AdministrativeInspection()
        {
            InvestigationStages = new List<InvestigationStage>();
            Investigators = new List<Investigator>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdministrativeInspectionId { get; set; }    
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public ICollection<InvestigationStage> InvestigationStages { get; set; }
        public Company Company { get; set; }
        public ICollection<Investigator> Investigators { get; set; }
        public Conclusion? Conclusion { get; set; }
    }
}
