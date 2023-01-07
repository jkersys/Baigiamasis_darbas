using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models
{
    public class Investigator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestigatorId { get; set; }
        public string CertificationId { get; set; }
        public string CabinetNumber { get; set; }
        public string WorkAdress { get; set; }
        public LocalUser LocalUser { get; set; }
        public int LocalUserId { get; set; }
        public virtual IEnumerable<Investigation> Investigations { get; set; }
        public virtual IEnumerable<AdministrativeInspection> AdministrativeInspections { get; set; }
        public virtual IEnumerable<Complain> Complains { get; set; }
    }
}
