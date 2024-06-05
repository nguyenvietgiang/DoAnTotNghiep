using Microsoft.Data.SqlClient;

namespace DoAnTotNghiep.Common
{
    public class BackupRestoreService
    {
        private readonly string _connectionString;

        public BackupRestoreService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Connect");
        }

        public async Task BackupDatabaseAsync(string backupPath)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("BackupDatabase", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BackupPath", backupPath);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task RestoreDatabaseAsync(string backupFilePath)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("RestoreDatabase", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BackupFilePath", backupFilePath);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
