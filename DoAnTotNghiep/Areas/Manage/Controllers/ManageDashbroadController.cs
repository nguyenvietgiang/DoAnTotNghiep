using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageDashbroadController : ManageBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
