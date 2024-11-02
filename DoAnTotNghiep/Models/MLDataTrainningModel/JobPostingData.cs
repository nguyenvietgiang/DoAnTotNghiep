namespace DoAnTotNghiep.Models.MLDataTrainningModel
{
    public class JobPostingData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Requirements { get; set; }
        public int Number { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public string Benefits { get; set; }
        public string WorkingTime { get; set; }
        public bool Status { get; set; } // Nhãn (label) cho việc huấn luyện
    }

}
