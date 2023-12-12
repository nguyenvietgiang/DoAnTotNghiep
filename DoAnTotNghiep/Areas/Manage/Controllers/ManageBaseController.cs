using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    public class ManageBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
