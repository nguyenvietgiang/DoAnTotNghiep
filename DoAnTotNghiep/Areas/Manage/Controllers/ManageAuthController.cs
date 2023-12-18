using DoAnTotNghiep.Common;
using DoAnTotNghiep.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageAuthController : Controller
    {
        private readonly DataContext _context;
        public ManageAuthController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Account rowuser = _context.Accounts.Where(m => m.Status == true && (m.Email == email)).FirstOrDefault();

            if (rowuser == null || rowuser.AccountRole != Models.Enum.AccountRole.Admin)
            {
                ViewBag.thongbao = "Sai tài khoản hoặc bạn không có quyền admin";
            }
            else
            {
                if ((rowuser.Password) == Encrypt.MD5Hash(password))
                {
                    HttpContext.Session.SetString("adminname", email);
                    HttpContext.Session.SetString("adminid", rowuser.UserID.ToString());
                    return RedirectToAction("Index", "ManageDashbroad", new { area = "Manage" });
                }
                else
                {
                    ViewBag.thongbao = "Mật khẩu sai rồi";
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("adminname");
            HttpContext.Session.Remove("adminid");
            return RedirectToAction("Login", new { area = "manage" });
        }
    }
}

