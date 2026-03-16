using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using FastFood.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FastFood.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Username == Username && x.Password == Password);

            if (user == null)
            {
                return Json(new { success = false, message = "Sai tài khoản hoặc mật khẩu" });
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Username);
            HttpContext.Session.SetString("UserRole", user.Role);

            return Json(new { success = true });
        }
        
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Register(User user, string ConfirmPassword)
        {
            if(user.Password != ConfirmPassword)
            {
                return Json(new { success = false, message = "Mật khẩu xác nhận không khớp" });
            }

            user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult ExitDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}