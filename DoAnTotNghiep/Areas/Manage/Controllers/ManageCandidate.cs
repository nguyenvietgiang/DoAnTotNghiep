using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.AccountRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageCandidate : ManageBaseController
    {
        private readonly DataContext _context;
        private readonly IAccountRepository _accountRepository;
        public ManageCandidate(DataContext context, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var candidates = _context.Candidates.Include(c => c.Account).ToList();
            int totalItemCount = _context.Candidates.Count();
            var pagedList = new StaticPagedList<Candidate>(candidates.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, totalItemCount);
            return View(pagedList);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(e => e.CandidateID == id);

            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        public async Task<IActionResult> ToggleAccountStatus(Guid Id)
        {
            await _accountRepository.UpdateAccountStatusAsync(Id);
            return RedirectToAction("Index"); 
        }
    }
}
