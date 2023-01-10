using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTP_Web_API.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int CompanyRegistrationNumber { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyAdress { get; set; }     
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public ICollection<Investigation> Investigations { get; set; }
        public ICollection<AdministrativeInspection> AdministrativeInspections { get; set; }


    }
}
