using Microsoft.ML.Data;

namespace DoAnTotNghiep.Models.MLDataTrainningModel
{
    public class JobApprovalPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Status { get; set; } // Giá trị dự đoán: true (phù hợp) hoặc false (không phù hợp)
    }
}
