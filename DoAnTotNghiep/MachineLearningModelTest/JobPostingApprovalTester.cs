using DoAnTotNghiep.Models.MLDataTrainningModel;

namespace DoAnTotNghiep.MachineLearningModelTest
{
    public class JobPostingApprovalTester
    {
        private JobPostingApprovalService _jobPostingApprovalService;

        public JobPostingApprovalTester()
        {
            // Khởi tạo dịch vụ kiểm duyệt bài đăng
            _jobPostingApprovalService = new JobPostingApprovalService();
        }

        public void TestPrediction()
        {
            // Tạo dữ liệu mẫu để dự đoán
            var jobPostingData = new JobPostingData
            {
                Title = "Software Engineer",
                Description = "Looking for a skilled software engineer with experience in C# and .NET.",
                Location = "Hanoi",
                Requirements = "C#, ASP.NET Core, SQL Server",
                Position = "Full-time",
                Benefits = "Health insurance, paid time off",
                Number = 3,
                Salary = 70000
            };

            // Dự đoán
            bool isApproved = _jobPostingApprovalService.Predict(jobPostingData);

            // Hiển thị kết quả
            Console.WriteLine($"Job Posting Approval Status: {(isApproved ? "Approved" : "Not Approved")}");
        }
    }
}
