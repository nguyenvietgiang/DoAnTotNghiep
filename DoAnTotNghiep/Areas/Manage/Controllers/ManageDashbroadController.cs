using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    public class ManageDashbroadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
