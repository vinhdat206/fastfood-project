using Microsoft.AspNetCore.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email,string password)
        {
            if(email=="admin@fastfood.com" && password=="123456")
            {
                HttpContext.Session.SetString("Admin","true");

                return RedirectToAction("Index","Dashboard");
            }

            ViewBag.Error="Sai tài khoản hoặc mật khẩu";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Login");
        }
    }
}