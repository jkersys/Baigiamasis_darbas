using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models.Dto.InvestigatorDto
{
    public class CreateInvestigatorDto
    {
        /// <summary>
        /// Tyrejo pazymejimo numeris
        /// </summary>
        [Required]
        public string CertificateNumber { get; set; }
        /// <summary>
        /// Kabineto numeris
        /// </summary>
        [Required]
        public string CabinetNumber { get; set; }
        /// <summary>
        /// Darbo vietos adresas
        /// </summary>
        [Required]
        public string WorkplaceAdress { get; set; }
        /// <summary>
        /// Jau sistemoje egzistuojanio vartotojo el pastas, su kuriuo yra sujungiami tyrejo duomenys
        /// </summary>
        [Required]
        public string LocalUserEmail { get; set; }
    }
}
