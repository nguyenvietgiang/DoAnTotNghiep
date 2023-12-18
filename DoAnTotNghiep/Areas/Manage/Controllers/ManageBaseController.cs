using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    public class ManageBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (HttpContext.Session.GetString("adminid") == null)
            {
                context.Result = new RedirectResult("/manage/ManageAuth/Login");
            }

        }
    }
}
