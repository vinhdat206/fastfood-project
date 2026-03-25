using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AdminController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
        {
            context.Result = RedirectToAction("Login", "Auth");
        }

        base.OnActionExecuting(context);
    }
    
    
}