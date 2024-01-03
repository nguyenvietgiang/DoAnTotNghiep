using DoAnTotNghiep.Models.EntityModels;

namespace DoAnTotNghiep.Services.ExportServices
{
    public interface IExcelExportService
    {
        byte[] ExportContactsToExcel(List<Contact> contacts);
    }
}
