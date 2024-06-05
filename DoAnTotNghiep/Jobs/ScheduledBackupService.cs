using Cronos;
using DoAnTotNghiep.Common;

namespace DoAnTotNghiep.Jobs
{
    public class ScheduledBackupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ScheduledBackupService> _logger;
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZoneInfo;

        public ScheduledBackupService(IServiceScopeFactory serviceScopeFactory, ILogger<ScheduledBackupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            // chạy mùng 5 hàng tháng
            _cronExpression = CronExpression.Parse("0 0 0 5 * *");
            //_cronExpression = CronExpression.Parse("*/10 * * * *"); // Chạy cứ mỗi 10 phút
            _timeZoneInfo = TimeZoneInfo.Local; // Sử dụng múi giờ cục bộ
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var next = _cronExpression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
                if (next.HasValue)
                {
                    var delay = next.Value - DateTimeOffset.Now;
                    if (delay > TimeSpan.Zero)
                    {
                        _logger.LogInformation($"Scheduled backup in {delay.TotalSeconds} seconds.");
                        await Task.Delay(delay, stoppingToken);
                    }

                    if (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            _logger.LogInformation("Starting scheduled backup.");

                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var backupRestoreService = scope.ServiceProvider.GetRequiredService<BackupRestoreService>();
                                await backupRestoreService.BackupDatabaseAsync("E:\\DataBackup");
                            }

                            _logger.LogInformation("Scheduled backup completed successfully.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Scheduled backup failed.");
                        }
                    }
                }
            }
        }
    }

}
