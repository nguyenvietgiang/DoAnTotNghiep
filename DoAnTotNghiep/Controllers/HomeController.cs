using DoAnTotNghiep.Models;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.ContactRepo;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DoAnTotNghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactRepository _contactRepository;
        public HomeController(ILogger<HomeController> logger, IContactRepository contactRepository )
        {
            _contactRepository= contactRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    Name = contactViewModel.Name,
                    Email = contactViewModel.Email,
                    Subject = contactViewModel.Subject,
                    Message = contactViewModel.Message
                };
                await _contactRepository.CreateAsync(contact);
                ViewBag.thongbao = "Thành công, cảm ơn bạn đã đóng góp phản hồi !!!";
            }
            return View(contactViewModel);
        }

        public IActionResult Forum()
        {
            return View();
        }

        public IActionResult Hiring()  
        {
            return View();
        }

        public IActionResult NoPermistion()
        {
            return View();
        }

        public IActionResult Sucess()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}