using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (HttpContext.Session.GetString("Accountid") == null)
            {
                context.Result = RedirectToAction("Login", "Auth");
            }

        }

        protected string GetUserIdFromClaim()
        {
            var accountIdClaim = HttpContext.Session.GetString("Accountid");
            return accountIdClaim;
        }
    }
}
