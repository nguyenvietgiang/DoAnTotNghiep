using DoAnTotNghiep.Models;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.ContactRepo;
using DoAnTotNghiep.Repository.DisscussRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Repository.JobApplyFormRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using System.Diagnostics;
using X.PagedList;

namespace DoAnTotNghiep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactRepository _contactRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly ICandidatesRepo _candidateRepository;
        private readonly IDiscussRepository _discussRepository;
        private readonly IJobApplyFormRepository _jobApplyFormRepository;
        public HomeController(ILogger<HomeController> logger, IContactRepository contactRepository, IEmployerRepository employerRepository, ICandidatesRepo candidatesRepo, IDiscussRepository discussRepository, IJobApplyFormRepository jobApplyFormRepository)
        {
            _employerRepository= employerRepository;
            _contactRepository= contactRepository;
            _candidateRepository = candidatesRepo;
            _discussRepository= discussRepository;
            _jobApplyFormRepository= jobApplyFormRepository;
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

        public async Task<IActionResult> Forum()
        {
            var discussList = await _discussRepository.GetAllDiscussions();
            return View(discussList);
        }

        public async Task<IActionResult> DetailDiscuss(Guid Id)
        {
            var discussList = await _discussRepository.GetDiscussionById(Id);
            return View(discussList);
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

        public async Task<IActionResult> EmployerList(int? page, string? searchTerm)
        {
            int pageSize = 9;
            int pageNumber = page ?? 1;
            var employers = await _employerRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                employers = employers.Where(e => e.CompanyName.Contains(searchTerm));
            }
            int totalItemCount = employers.Count();
            var pagedList = new StaticPagedList<Employer>(employers.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public async Task<IActionResult> CandidatesList(int? page, string? searchTerm)
        {
            int pageSize = 9; 
            int pageNumber = page ?? 1;
            var candidates = await _candidateRepository.GetAllAsync();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                candidates = candidates.Where(e => e.Name.Contains(searchTerm));
            }
            int totalItemCount = candidates.Count();
            var pagedList = new StaticPagedList<Candidate>(candidates.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        [HttpPost]
        public JsonResult ApplyForJob(JobApplyFormDTO jobApplyFormDTO)
        {
            // Thêm hồ sơ ứng tuyển vào cơ sở dữ liệu
            _jobApplyFormRepository.AddJobApplyForm(jobApplyFormDTO);
            return Json(new { success = true, message = "Ứng tuyển thành công" });
        }
    }
}