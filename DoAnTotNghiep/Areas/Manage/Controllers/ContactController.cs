using DoAnTotNghiep.Controllers;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.ContactRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController( IContactRepository contactRepository)
        {
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


    }
}
