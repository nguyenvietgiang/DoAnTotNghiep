using DNTCaptcha.Core;
using DoAnTotNghiep.Common;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class AuthController : Controller
    {
        private readonly DataContext _dbContext;

        public AuthController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModels model)
        {
            if (ModelState.IsValid)
            {
                Account rowuser = _dbContext.Accounts.Where(m => m.Status == true && (m.Email == model.Email)).FirstOrDefault();
                if (rowuser == null)
                {
                    ViewBag.thongbao = "Tài khoản này không tồn tại";
                }
                else
                {
                    if ((rowuser.Password) == Encrypt.MD5Hash(model.Password))
                    {
                        if (rowuser.AccountRole == AccountRole.EmployerFree || rowuser.AccountRole == AccountRole.EmployerPaid)
                        {
                            HttpContext.Session.SetString("EmployerName", model.Email);
                            HttpContext.Session.SetString("Accountid", rowuser.UserID.ToString());
                            return RedirectToAction("Index", "Home");
                        }
                        else if ((rowuser.AccountRole == AccountRole.CandidateFree || rowuser.AccountRole == AccountRole.CandidatePaid))
                        {
                            HttpContext.Session.SetString("CandidateName", model.Email);
                            HttpContext.Session.SetString("Accountid", rowuser.UserID.ToString());
                            return RedirectToAction("Index", "Home");
                        }    
                    }
                    else
                    {
                        ViewBag.thongbao = "Mật khẩu sai rồi";
                    }
                }
                return View();

            }
            return View();
        }

        public IActionResult Register() 
        {
            return View();
        }

        [HttpPost]
        [ValidateDNTCaptcha(ErrorMessage ="Mã Captcha không chính xác")]
        public IActionResult Register(RegisterViewModel model)
        {
            Account rowuser = _dbContext.Accounts.Where(m => m.Email == model.Email).FirstOrDefault();
            if (rowuser != null)
            {
                ViewBag.thongbao = "Email này đã được dùng bởi một tài khoản khác";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                string hashedPassword = Encrypt.MD5Hash(model.Password);
                // Kiểm tra loại tài khoản
                if (model.AccountRole == AccountRole.CandidateFree)
                {
                    var candidate = new Candidate
                    {
                        Name = model.Name,
                        Descrpitons = "Chưa cập nhật",
                        UrlImage = "/local-img/default.jpg",
                        DateOfBirth = new DateTime(2000, 1, 1),
                        PhoneNumber = 0,
                        Industry = "Chưa cập nhật",
                        Experience = 0,
                        EducationLevel = "Chưa cập nhật"
                    };

                    var account = new Account
                    {
                        Email = model.Email,
                        Password = hashedPassword,
                        AccountRole = model.AccountRole,
                        Candidate = candidate,
                        Status = true
                    };

                    _dbContext.Accounts.Add(account);
                }
                else if(model.AccountRole == AccountRole.EmployerFree)
                {
                    var employer = new Employer
                    {
                        CompanyName = model.Name,
                        UrlImage = "/local-img/default.jpg",
                        Industry = "Chưa cập nhật",
                        CompanySize = 0,
                        Descrpitons ="Chưa cập nhật",
                        Location = "Chưa cập nhật",
                        PhoneNumber = "Chưa cập nhật"
                    };

                    var account = new Account
                    {
                        Email = model.Email,
                        Password = hashedPassword,
                        AccountRole = model.AccountRole,
                        Employer = employer,
                        Status = true
                    };

                    _dbContext.Accounts.Add(account);
                }
                _dbContext.SaveChanges();
                ViewBag.thongbao = "Tài khoản đã được đăng ký thành công!!!";
            }
            return View(model);
        }

        public IActionResult ForgotPass() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("EmployerName");
            HttpContext.Session.Remove("CandidateName");
            HttpContext.Session.Remove("Accountid");
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Changepass(IFormCollection fielt)
        {
            var accountIdClaim = HttpContext.Session.GetString("Accountid");
            if(accountIdClaim == null)
            {
                return RedirectToAction("Login");
            }
            string old = fielt["oldpass"];
            string newpass = fielt["pass"];
            string repass = fielt["repass"];
            var userchangpass = await _dbContext.Accounts
                .FirstOrDefaultAsync(m => m.UserID.ToString() == accountIdClaim);
            if (Encrypt.MD5Hash(old) == userchangpass.Password)
            {
                if (newpass == repass)
                {
                    userchangpass.Password = Encrypt.MD5Hash(newpass);
                    _dbContext.Update(userchangpass);
                    await _dbContext.SaveChangesAsync();
                    ViewBag.thongbao = "Thay Đổi Mật Khẩu Thành Công, Áp Dụng Cho Lần Tiếp Theo !!!";
                }
                else
                {
                    ViewBag.thongbao = "Mật Khẩu Nhập Lại Không Chính Xác";
                }
            }
            else
            {
                ViewBag.thongbao = "Mật Khẩu Cũ Không Chính Xác !!!";
            }
            return View();
        }

    }
}
