using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Candidate
    {
        [Key]
        public Guid CandidateID { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Descrpitons { get; set; }

        public string? UrlImage { get; set; }

        public DateTime? DateOfBirth { get; set; } 
        public int PhoneNumber { get; set; }
        // ngành nghề quan tâm
        public string? Industry { get; set; } 
        public int Experience { get; set; } 
        public string? EducationLevel { get; set; }

        [ForeignKey("CandidateID")]
        public Account Account { get; set; }
     
    }
}
