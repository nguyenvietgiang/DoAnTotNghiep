using DoAnTotNghiep.Models.MLDataTrainningModel;
using Microsoft.ML;

public class JobPostingApprovalService
{
    private readonly MLContext _mlContext;
    // tệp chứa mô hình sau khi đã được huấn luyện
    private readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Models", "JobApprovalModel.zip");

    private ITransformer _model;

    public JobPostingApprovalService()
    {
        _mlContext = new MLContext();
        if (File.Exists(_modelPath))
        {
            LoadModel();
        }
        else
        {
            TrainModel();
        }
    }

    // Huấn luyện mô hình
    public void TrainModel()
    {
        IDataView dataView = _mlContext.Data.LoadFromTextFile<JobPostingData>("jobpostings.csv", separatorChar: ',', hasHeader: true);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("TitleFeaturized", nameof(JobPostingData.Title))
            .Append(_mlContext.Transforms.Text.FeaturizeText("DescriptionFeaturized", nameof(JobPostingData.Description)))
            .Append(_mlContext.Transforms.Text.FeaturizeText("LocationFeaturized", nameof(JobPostingData.Location)))
            .Append(_mlContext.Transforms.Text.FeaturizeText("RequirementsFeaturized", nameof(JobPostingData.Requirements)))
            .Append(_mlContext.Transforms.Text.FeaturizeText("PositionFeaturized", nameof(JobPostingData.Position)))
            .Append(_mlContext.Transforms.Text.FeaturizeText("BenefitsFeaturized", nameof(JobPostingData.Benefits)))
            .Append(_mlContext.Transforms.Concatenate("Features", "TitleFeaturized", "DescriptionFeaturized",
                                                     "LocationFeaturized", "RequirementsFeaturized",
                                                     "PositionFeaturized", "BenefitsFeaturized",
                                                     nameof(JobPostingData.Number), nameof(JobPostingData.Salary)))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(JobPostingData.Status)))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        _model = pipeline.Fit(dataView);

        _mlContext.Model.Save(_model, dataView.Schema, _modelPath);
    }

    // Dự đoán
    public bool Predict(JobPostingData jobPostingData)
    {
        if (_model == null) LoadModel();
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<JobPostingData, JobApprovalPrediction>(_model);
        var prediction = predictionEngine.Predict(jobPostingData);
        return prediction.Status;
    }

    private void LoadModel()
    {
        _model = _mlContext.Model.Load(_modelPath, out _);
    }
}
