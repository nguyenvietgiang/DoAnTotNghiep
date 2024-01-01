namespace DoAnTotNghiep.Models.ResponseDTO
{
    public class JobPostingResponseDto
    {
        public Guid JobId { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string Image { get; set; }

    }
}
