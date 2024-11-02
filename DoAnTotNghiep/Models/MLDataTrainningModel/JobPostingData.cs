using Microsoft.ML.Data;

namespace DoAnTotNghiep.Models.MLDataTrainningModel
{
    public class JobPostingData
    {
        [LoadColumn(0)]
        public string Title { get; set; }

        [LoadColumn(1)]
        public string Description { get; set; }

        [LoadColumn(2)]
        public string Location { get; set; }

        [LoadColumn(3)]
        public string Requirements { get; set; }

        [LoadColumn(4)]
        public string Position { get; set; }

        [LoadColumn(5)]
        public string Benefits { get; set; }

        [LoadColumn(6)]
        public float Number { get; set; }

        [LoadColumn(7)]
        public float Salary { get; set; }

        [LoadColumn(8)]
        public bool Status { get; set; }
    }

}
