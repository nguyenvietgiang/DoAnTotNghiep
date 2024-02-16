using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.AccountRepo;
using DoAnTotNghiep.Repository.FollowRepo;
using DoAnTotNghiep.Services.EmailServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageEmployerController : ManageBaseController
    {
        private readonly DataContext _context;
        private readonly IAccountRepository _accountRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IEmailServices _emailServices;
        public ManageEmployerController(DataContext context, IAccountRepository accountRepository, IFollowRepository followRepository,IEmailServices emailServices)
        {
            _accountRepository= accountRepository;
            _followRepository= followRepository;
            _emailServices = emailServices;
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var employers = _context.Employers.Include(c => c.Account).ToList();
            int totalItemCount = _context.Employers.Count();
            var pagedList = new StaticPagedList<Employer>(employers.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public async Task<IActionResult> ToggleAccountStatus(Guid Id)
        {
            await _accountRepository.UpdateAccountStatusAsync(Id);
            //var followers = _followRepository.GetFollowers(Id);

            //if (followers != null)
            //{
            //    foreach (var follower in followers)
            //    {
            //        _emailServices.SendEmailAsync(follower.Email, "Công ty bạn đang theo dõi đã bị khóa");
            //    }
            //}
            return RedirectToAction("Index");
        }
    }
}
