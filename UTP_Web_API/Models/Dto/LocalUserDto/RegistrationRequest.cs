using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models.Dto.LocalUserDto
{
    public class RegistrationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public long PhoneNumber { get; set; }

    }
}
