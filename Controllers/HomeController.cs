using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastFood.Models;
using FastFood.Data;

namespace FastFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // HOME PAGE
        public async Task<IActionResult> Index()
        {
            var foods = await _context.Foods
                .Include(x => x.Category) // 🔥 QUAN TRỌNG
                .ToListAsync();

            // 🔥 sản phẩm thường (không phải combo)
            var normalFoods = foods
                .Where(x => x.Category.CategoryName != "Combo")
                .ToList();

            var bestSeller = normalFoods
                .OrderBy(x => Guid.NewGuid())
                .Take(8)
                .ToList();

            var saleProducts = normalFoods
                .Where(x => x.DiscountPercent > 0) // 🔥 CHỈ LẤY HÀNG GIẢM GIÁ
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .ToList();

            // 🔥 combo riêng
            var comboProducts = foods
                .Where(x => x.Category.CategoryName == "Combo")
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .ToList();

            ViewBag.BestSeller = bestSeller;
            ViewBag.SaleProducts = saleProducts;
            ViewBag.ComboProducts = comboProducts; // 🔥 THÊM DÒNG NÀY

            return View(foods);
        }

        // ABOUT PAGE
        public IActionResult About()
        {
            return View();
        }

        // CONTACT PAGE
        public IActionResult Contact()
        {
            return View();
        }

        // SAVE CONTACT MESSAGE
        [HttpPost]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;

                _context.ContactMessages.Add(model);

                await _context.SaveChangesAsync();

                ViewBag.Success = "Gửi liên hệ thành công!";
            }

            return View();
        }

        // POPUP FOOD DETAIL
        
        
        public IActionResult FoodDetail(int id)
        {
            var food = _context.Foods.Find(id);

            if (food == null)
            {
                return NotFound();
            }

            return PartialView("_FoodDetail", food);
        }

        // PRIVACY PAGE
        public IActionResult Policy()
        {
            return View();
        }

        // ERROR PAGE
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}