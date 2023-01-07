﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UTP_Web_API.Models
{
    public class Investigation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvestigationId { get; set; }
        public int CompanyId { get; set; }
        public string LegalBase { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Conclusion Conclusion { get; set; }
        public int Penalty { get; set; }
        public IEnumerable<InvestigationStage> Stages { get; set; }
        public IEnumerable<Investigator> Investigators { get; set; }
        public Company Company { get; set; }
    }
}
