using DNTCaptcha.Core;
using DoAnTotNghiep.Common;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using DoAnTotNghiep.Services.EmailServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class AuthController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IEmailServices _emailServices;

        public AuthController(DataContext dbContext, IEmailServices emailServices)
        {
            _emailServices= emailServices;
            _dbContext = dbContext;
        }

        public IActionResult Login()
        {
            // Kiểm tra xem đã có cookie nhớ tài khoản hay không
            if (Request.Cookies.ContainsKey("RememberMe"))
            {
                ViewBag.RememberMe = true;
                ViewBag.Email = Request.Cookies["Email"];
                ViewBag.Password = Request.Cookies["Password"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModels model, bool rememberMe)
        {
            if (ModelState.IsValid)
            {
                Account rowuser = _dbContext.Accounts.FirstOrDefault(m => m.Email == model.Email);

                if (rowuser == null)
                {
                    ViewBag.thongbao = "Tài khoản này không tồn tại";
                }
                else
                {
                    if (!rowuser.Status)
                    {
                        ViewBag.thongbao = "Tài khoản của bạn đã bị khóa, hãy liên hệ với chúng tôi để biết thêm thông tin";
                    }
                    else if (rowuser.Password == Encrypt.MD5Hash(model.Password))
                    {
                        if (rememberMe)
                        {
                            // Nếu người dùng chọn nhớ tài khoản, lưu thông tin vào cookies
                            Response.Cookies.Append("RememberMe", "true");
                            Response.Cookies.Append("Email", model.Email);
                            Response.Cookies.Append("Password", model.Password);
                        }

                        HttpContext.Session.SetString("Accountid", rowuser.UserID.ToString());
                        HttpContext.Session.SetInt32("UserRole", (int)rowuser.AccountRole);

                        if (rowuser.AccountRole == AccountRole.EmployerFree || rowuser.AccountRole == AccountRole.EmployerPaid)
                        {
                            HttpContext.Session.SetString("EmployerName", model.Email);
                        }
                        else if (rowuser.AccountRole == AccountRole.CandidateFree || rowuser.AccountRole == AccountRole.CandidatePaid)
                        {
                            HttpContext.Session.SetString("CandidateName", model.Email);
                        }

                        return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public async Task<IActionResult> Forgotpass(IFormCollection fielt)
        {
            string mail = fielt["email"];
            Account rowuser = _dbContext.Accounts.Where(m => m.Email == mail).FirstOrDefault();
            if (rowuser != null)
            {
                Random r = new Random();
                int i = r.Next(1, 99999);
                rowuser.Password = Encrypt.MD5Hash(i.ToString());
                _dbContext.Update(rowuser);
                await _dbContext.SaveChangesAsync();
                string message = "Mật khẩu mới của bạn là: "+ i;
                await _emailServices.SendEmailAsync(mail, message);
                ViewBag.ancap = "Hãy Kiểm Tra Email";
            }
            else
            {
                ViewBag.ancap = "Email này chưa được liên kết với tài khoản nào";
            }
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
            if (HttpContext.Session.GetString("Accountid") == null)
            {
                return View("Login");
            }
            else
            {
                return View();
            }
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
