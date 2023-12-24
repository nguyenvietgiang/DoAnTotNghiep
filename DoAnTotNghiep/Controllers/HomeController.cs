using DoAnTotNghiep.Models;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.ContactRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DoAnTotNghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly ICandidatesRepo _candidateRepository;
        public HomeController(ILogger<HomeController> logger, IContactRepository contactRepository, IEmployerRepository employerRepository, ICandidatesRepo candidatesRepo )
        {
            _employerRepository= employerRepository;
            _contactRepository= contactRepository;
            _candidateRepository = candidatesRepo;
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
                    Message = contactViewModel.Message,
                    Status = false
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

        public IActionResult Conpany(Guid Id) 
        {
            var employer = _employerRepository.GetEmployerByIdAsync(Id).Result;

            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        public IActionResult UserProfile(Guid Id) 
        {
            var candidate = _candidateRepository.GetCandidateByIdAsync(Id).Result;

            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

    }
}