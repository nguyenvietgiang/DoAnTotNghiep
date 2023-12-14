using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class EmployerController : Controller
    {
        public IActionResult CompanyProfile() 
        {
            return View();
        }
    }
}
