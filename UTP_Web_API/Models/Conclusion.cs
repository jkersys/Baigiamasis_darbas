using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models
{
    public class Conclusion
    {
        public Conclusion()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConclusionId { get; set; }
        public string Decision { get; set; }
        public ICollection<Complain>? Complains { get; set; }
        public ICollection<AdministrativeInspection>? AdministrativeInspections { get; set; }
        public ICollection<Investigation>? Investigations { get; set; }
    }
}
