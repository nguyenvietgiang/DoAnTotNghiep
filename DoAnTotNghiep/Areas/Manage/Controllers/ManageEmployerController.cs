using DoAnTotNghiep.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageEmployerController : ManageBaseController
    {
        private readonly DataContext _context;

        public ManageEmployerController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var employers = _context.Employers.ToList();
            int totalItemCount = _context.Employers.Count();
            var pagedList = new StaticPagedList<Employer>(employers.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }
    }
}
