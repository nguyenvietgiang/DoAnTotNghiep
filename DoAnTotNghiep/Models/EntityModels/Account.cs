using DoAnTotNghiep.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Account
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public AccountRole AccountRole { get; set; }

        public Candidate Candidate { get; set; }
        public Employer Employer { get; set; }

        public bool Status { get; set; }

        public ICollection<Message> SentMessages { get; set; }

        public ICollection<Message> ReceivedMessages { get; set; }

        public List<Discuss> Discusses { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<Follow> Follows { get; set; }
    }
}
