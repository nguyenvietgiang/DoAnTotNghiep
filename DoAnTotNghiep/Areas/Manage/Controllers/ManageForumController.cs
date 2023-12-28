using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.DisscussRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageForum : ManageBaseController
    {
        private readonly IDiscussRepository _discussRepository;

        public ManageForum(IDiscussRepository discussRepository)
        {
            _discussRepository = discussRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var disscuses = await _discussRepository.GetAllDiscussions();
            int totalItemCount = disscuses.Count();
            var pagedList = new StaticPagedList<Discuss>(disscuses.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }
    }
}
