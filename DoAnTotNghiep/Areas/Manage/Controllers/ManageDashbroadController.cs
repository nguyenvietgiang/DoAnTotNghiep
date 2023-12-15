using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageDashbroadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
