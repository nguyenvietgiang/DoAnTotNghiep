using DoAnTotNghiep.Controllers;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.ContactRepo;
using DoAnTotNghiep.Services.EmailServices;
using DoAnTotNghiep.Services.ExportServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ContactController : ManageBaseController
    {
        private readonly IContactRepository _contactRepository;
        private readonly IEmailServices _emailServices;
        private readonly IExcelExportService _excelExportService;
        public ContactController( IContactRepository contactRepository, IEmailServices emailServices, IExcelExportService excelExportService)
        {
            _excelExportService= excelExportService;
            _emailServices= emailServices;
            _contactRepository = contactRepository;
        }
        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var contacts = await _contactRepository.GetAllAsync();
            int totalItemCount = contacts.Count(); 
            var pagedList = new StaticPagedList<Contact>(contacts.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public async Task<IActionResult> ContactResponse(Guid id) 
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> ContactResponse(Guid id, string repcontent)
        {
            var contact = await _contactRepository.GetByIdAsync(id);
            await _emailServices.SendEmailAsync(contact.Email, repcontent);
            await _contactRepository.ToggleStatusAsync(id);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _contactRepository.DeleteAsync(id);
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error deleting contact: {ex.Message}";
                return View("Error");
            }
        }
        public async Task<IActionResult> ExportToExcel()
        {
            IEnumerable<Contact> contactsEnumerable = await _contactRepository.GetAllAsync();
            List<Contact> contacts = contactsEnumerable.ToList();
            byte[] excelData = _excelExportService.ExportContactsToExcel(contacts);
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contacts.xlsx");
        }

    }
}
