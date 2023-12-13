using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class CandidatesController : Controller
    {
        public IActionResult Profile() 
        {
            return View();
        }
    }
}
