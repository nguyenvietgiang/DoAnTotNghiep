using System.ComponentModel.DataAnnotations;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Timestamp] // Thuộc tính này dùng để theo dõi thay đổi dữ liệu
        public byte[] RowVersion { get; set; }
    }

}
