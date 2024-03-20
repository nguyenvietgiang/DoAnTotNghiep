using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Models.ResponseDTO
{
    public class QuestionWithOptionCountsViewModel
    {
        public Question Question { get; set; }
        public List<OptionCountViewModel> OptionCounts { get; set; }
    }

    public class OptionCountViewModel
    {
        public Option Option { get; set; }
        public int Count { get; set; }
    }
}
