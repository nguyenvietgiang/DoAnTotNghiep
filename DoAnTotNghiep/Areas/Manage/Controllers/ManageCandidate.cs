using DoAnTotNghiep.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageCandidate : Controller
    {
        private readonly DataContext _context;

        public ManageCandidate(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var candidates = _context.Candidates.ToList();
            int totalItemCount = _context.Candidates.Count();
            var pagedList = new StaticPagedList<Candidate>(candidates.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }
    }
}
