using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    public class ManageAuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
