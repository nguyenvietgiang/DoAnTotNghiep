using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
