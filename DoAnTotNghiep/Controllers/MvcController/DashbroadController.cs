using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.Enum;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class DashbroadController : BaseController
    {
        private readonly DataContext _dataContext;

        public DashbroadController(DataContext dataContext)
        {
            _dataContext= dataContext;
        }
        public IActionResult Index()
        {
            var userId = GetUserIdFromClaim();
            var account = _dataContext.Accounts.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            if (account.AccountRole == AccountRole.CandidateFree || account.AccountRole == AccountRole.EmployerFree)
            {
                return RedirectToAction("NoPermistion", "Home");
            }
            return View();
        }
    }
}
