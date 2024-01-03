using DoAnTotNghiep.Models.EntityModels;
using Syncfusion.XlsIO;

namespace DoAnTotNghiep.Services.ExportServices
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportContactsToExcel(List<Contact> contacts)
        {
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                // Create a new workbook
                IWorkbook workbook = application.Workbooks.Create();

                // Access the first worksheet
                IWorksheet worksheet = workbook.Worksheets[0];

                // Add headers
                worksheet[1, 1].Text = "ID";
                worksheet[1, 2].Text = "Name";
                worksheet[1, 3].Text = "Email";
                worksheet[1, 4].Text = "Subject";
                worksheet[1, 5].Text = "Message";
                worksheet[1, 6].Text = "Status";

                // Add data
                for (int i = 0; i < contacts.Count; i++)
                {
                    worksheet[i + 2, 1].Text = contacts[i].Id.ToString();
                    worksheet[i + 2, 2].Text = contacts[i].Name;
                    worksheet[i + 2, 3].Text = contacts[i].Email;
                    worksheet[i + 2, 4].Text = contacts[i].Subject;
                    worksheet[i + 2, 5].Text = contacts[i].Message;
                    worksheet[i + 2, 6].Text = contacts[i].Status.ToString();
                }

                // Save the workbook to a MemoryStream
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
