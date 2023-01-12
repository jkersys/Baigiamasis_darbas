using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTP_Web_API.Models
{
    public class LocalUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(70)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(70)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(30)]
        public long PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }      
        public UserRole Role { get; set; }              
        public IEnumerable<Complain>? Complains { get; set; }
        public Investigator? Investigator { get; set; }
    }
}
