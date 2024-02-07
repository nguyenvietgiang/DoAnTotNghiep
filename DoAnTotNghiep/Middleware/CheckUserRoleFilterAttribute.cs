using DoAnTotNghiep.Models.Enum;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Middleware
{
    public class CheckUserRoleFilterAttribute : ActionFilterAttribute
    {
        private readonly AccountRole[] _allowedRoles;

        public CheckUserRoleFilterAttribute(params AccountRole[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var accountIdClaim = context.HttpContext.Session.GetInt32("UserRole");
            if (accountIdClaim == null || !_allowedRoles.Contains((AccountRole)accountIdClaim))
            {
                context.Result = new RedirectToActionResult("Index", "Error", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
