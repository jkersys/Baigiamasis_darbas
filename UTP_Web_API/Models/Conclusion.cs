using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models
{
    public class Conclusion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConclusionId { get; set; }
        public string Decision { get; set; }
        public IEnumerable<Complain> Complains { get; set; }
        public IEnumerable<AdministrativeInspection> AdministrativeInspections { get; set; }
        public IEnumerable<Investigation> Investigations { get; set; }
    }
}
