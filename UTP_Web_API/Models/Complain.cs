using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models
{
    public class Complain
    {
        public Complain()
        {
            Stages = new List<InvestigationStage>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComplainId { get; set; }       
        [Required]
        public string Description { get; set; }
        [Required]
        public string CompanyInformation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Conclusion? Conclusion { get; set; }
        public LocalUser LocalUser { get; set; }
        public Investigator? Investigator { get; set; }
        public ICollection<InvestigationStage> Stages { get; set; }
    }
}
