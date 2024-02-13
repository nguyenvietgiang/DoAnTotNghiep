using DoAnTotNghiep.Models;
using DoAnTotNghiep.Models.DTO;
using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CandidatesRepo;
using DoAnTotNghiep.Repository.CommentRepo;
using DoAnTotNghiep.Repository.ContactRepo;
using DoAnTotNghiep.Repository.DisscussRepo;
using DoAnTotNghiep.Repository.EmployerRepo;
using DoAnTotNghiep.Repository.FollowRepo;
using DoAnTotNghiep.Repository.ImageGaleryRepo;
using DoAnTotNghiep.Repository.JobApplyFormRepo;
using DoAnTotNghiep.Repository.JobRepo;
using DoAnTotNghiep.Repository.PolicyRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
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
        private readonly IFollowRepository _followRepository;
        private readonly IImageGaleryRepository _imageGaleryRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly DataContext _dataContext;
        public HomeController(ILogger<HomeController> logger, IContactRepository contactRepository, IEmployerRepository employerRepository, ICandidatesRepo candidatesRepo
            , IDiscussRepository discussRepository, IJobApplyFormRepository jobApplyFormRepository, IJobPostingRepository jobPostingRepository, ICommentRepository commentRepository,
            IFollowRepository followRepository, IImageGaleryRepository imageGaleryRepository, DataContext dataContext, IPolicyRepository policyRepository)
        {
            _employerRepository= employerRepository;
            _contactRepository= contactRepository;
            _candidateRepository = candidatesRepo;
            _discussRepository= discussRepository;
            _jobApplyFormRepository= jobApplyFormRepository;
            _followRepository = followRepository;
            _imageGaleryRepository= imageGaleryRepository;
            _policyRepository= policyRepository;
            _dataContext= dataContext;
            _jobPostingRepository= jobPostingRepository;
            _commentRepository= commentRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Employer = _dataContext.Employers.Count();
            ViewBag.Cadidates = _dataContext.Candidates.Count();
            ViewBag.Job = _dataContext.JobPostings.Count();
            ViewBag.Disscus = _dataContext.Discusses.Count();
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

        public async Task<IActionResult> Forum(string? searchTerm)
        {
            var discussList = await _discussRepository.GetApprovedDiscussionsWithCounts();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var filteredList = discussList.Where(e => e.Discuss.Title.Contains(searchTerm)).ToList();
                return View(filteredList);
            }

            return View(discussList);
        }

        public async Task<IActionResult> DetailDiscuss(Guid Id)
        {
            var discussList = await _discussRepository.GetDiscussionById(Id);
            return View(discussList);
        }

        // Action để hiển thị và lọc danh sách công việc
        public async Task<IActionResult> Hiring(string title, string location, int? salary, string time)
        {
            var accountIdClaim = HttpContext.Session.GetString("Accountid");
            var user = await _dataContext.Accounts
               .FirstOrDefaultAsync(m => m.UserID.ToString() == accountIdClaim);

            string recomment = null; // Mặc định recomment là null

            // Kiểm tra xem user và user.Candidate có null hay không trước khi gán recomment
            if (user != null && user.Candidate != null)
            {
                recomment = user.Candidate.Industry;
            }

            var jobPostings = await _jobPostingRepository.GetFilteredJobPostingsAsync(title, location, salary, time, recomment);
            string filterMessage = "Tất cả công việc";
            if (!string.IsNullOrEmpty(title))
            {
                filterMessage = $"Công việc liên quan đến '{title}'";
                if (!string.IsNullOrEmpty(location))
                {
                    filterMessage += $" tại {location}";
                }
            }
            else if (!string.IsNullOrEmpty(location))
            {
                filterMessage = $"Danh sách công việc tại {location}";
            }

            ViewData["TitleJob"] = $"Danh sách - {filterMessage}";
            ViewData["JobCount"] = jobPostings.Count();

            return View(jobPostings);
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

        public async Task<IActionResult> Conpany(Guid Id) 
        {
            var employer = _employerRepository.GetEmployerByIdAsync(Id).Result;

            if (employer == null)
            {
                return NotFound();
            }
            var followCount = await _followRepository.GetFollowCount(Id);
            ViewBag.FollowCount = followCount;
            ViewBag.GaleryList = await _imageGaleryRepository.GetImageGaleriesByEmployerIdAsync(Id);
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

        public async Task<IActionResult> AddFollow(Guid followUserId)
        {
            var accountIdClaim = HttpContext.Session.GetString("Accountid");
            if (accountIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            Guid userId = Guid.Parse(accountIdClaim);
            bool isFollowing = await _followRepository.IsFollowing(userId, followUserId);

            if (isFollowing)
            {
                // Nếu đang follow, gọi hàm RemoveFollow để hủy follow
                await _followRepository.RemoveFollow(userId, followUserId);
            }
            else
            {
                // Nếu chưa follow, gọi hàm AddFollow để thêm follow
                await _followRepository.AddFollow(userId, followUserId);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Policy() 
        {
            var policies = await _policyRepository.GetAllPoliciesAsync();
            return View(policies);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsForDiscuss(Guid discussId)
        {
            var comments = await _commentRepository.GetCommentsForDiscussAsync(discussId);

            var commentsInfo = comments.Select(c => new
            {
                UserName = c.Account.Email,
                Content = c.Content,
                CreatedAt = c.CreatedAt.ToShortDateString()
            });

            return Json(commentsInfo);
        }
    }
}