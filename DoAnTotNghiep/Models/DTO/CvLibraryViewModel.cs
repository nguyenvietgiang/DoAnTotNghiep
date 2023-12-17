namespace DoAnTotNghiep.Models.DTO
{
    public class CvLibraryViewModel
    {
        public string CvName { get; set; }

        public string CvType { get; set; }

        public IFormFile ImageFile { get; set; }

        public IFormFile PdfFile { get; set; }
    }
}
