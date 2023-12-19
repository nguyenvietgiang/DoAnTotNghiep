namespace DoAnTotNghiep.Models.DTO
{
    public class CandidateEditViewModels
    {
        public Guid CandidateID { get; set; }
        public string Name { get; set; }
        public string? Descrpitons { get; set; }
        public IFormFile? NewImage { get; set; }
        public string? UrlImage { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int PhoneNumber { get; set; }
        public string? Industry { get; set; }
        public int Experience { get; set; }
        public string? EducationLevel { get; set; }
    }
}
