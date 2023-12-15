using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên của bạn.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vấn đề.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung góp ý.")]
        public string Message { get; set; }
    }
}
