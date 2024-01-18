using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Policy
    {
        [Key]
        public Guid PolicyID { get; set; }

        public string? PolicyTitle { get; set; }

        public string? Content { get; set; }
    }
}
