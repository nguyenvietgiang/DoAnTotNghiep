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
                        if (rowuser.AccountRole == AccountRole.EmployerFree)
                        {
                            HttpContext.Session.SetString("Employerid", rowuser.UserID.ToString());
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            HttpContext.Session.SetString("Candidateid", rowuser.Candidate.ToString());
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
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = Encrypt.MD5Hash(model.Password);
                // Kiểm tra loại tài khoản
                if (model.AccountRole == AccountRole.CandidateFree)
                {
                    var candidate = new Candidate
                    {
                        Name = "Chưa cập nhật",
                        Descrpitons = "Chưa cập nhật",
                        UrlImage = "Chưa cập nhật",
                        DateOfBirth = null,
                        PhoneNumber = 0,
                        Industry = "Chưa cập nhật",
                        Experience = 0,
                        EducationLevel = null
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
                else if (model.AccountRole == AccountRole.EmployerFree)
                {
                    var employer = new Employer
                    {
                        CompanyName = "Chưa cập nhật",
                        UrlImage = "Chưa cập nhật",
                        Industry = "Chưa cập nhật",
                        CompanySize = null,
                        Location = "Chưa cập nhật"
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

                // Redirect hoặc thực hiện các hành động khác sau khi đăng ký thành công
                return RedirectToAction("Index", "Home");
            }
            // Nếu ModelState không hợp lệ, quay lại trang đăng ký với thông tin lỗi
            return View(model);
        }

        public IActionResult ForgotPass() 
        {
            return View();
        }
    }
}
