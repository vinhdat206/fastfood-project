using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using FastFood.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ALL FOOD
        public IActionResult Index()
        {
            ViewBag.Active = "All";
            var foods = _context.Foods.ToList();
            return View(foods);
        }

        // BURGER
        public IActionResult Burger()
        {
            ViewBag.Active = "Burger";

            var burgers = _context.Foods
                .Where(f => f.CategoryId == 1)
                .ToList();

            return View("Index", burgers);
        }

        // PIZZA
        public IActionResult Pizza()
        {
            ViewBag.Active = "Pizza";

            var pizzas = _context.Foods
                .Where(f => f.CategoryId == 2)
                .ToList();

            return View("Index", pizzas);
        }

        // CHICKEN
        public IActionResult Chicken()
        {
            ViewBag.Active = "Chicken";

            var chicken = _context.Foods
                .Where(f => f.CategoryId == 3)
                .ToList();

            return View("Index", chicken);
        }

        // DRINK
        public IActionResult Drink()
        {
            ViewBag.Active = "Drink";

            var drinks = _context.Foods
                .Where(f => f.CategoryId == 4)
                .ToList();

            return View("Index", drinks);
        }

        // COMBO
        public IActionResult Combo()
        {
            ViewBag.Active = "Combo";

            var combos = _context.Foods
                .Where(f => f.CategoryId == 5)
                .ToList();

            return View("Index", combos);
        }
        
        // public IActionResult SeaFood()
        // {
        //     ViewBag.Active = "Seafood";
        //
        //     var seafoods = _context.Foods
        //         .Where(f => f.CategoryId == 1001)
        //         .ToList();
        //
        //     return View("Index", seafoods);
        // }

        // POPUP FOOD DETAIL
        public IActionResult FoodDetail(int id)
        {
            var food = _context.Foods
                .AsNoTracking()
                .FirstOrDefault(f => f.FoodId == id);

            if (food == null)
                return NotFound();

            return PartialView("_FoodDetail", food);
        }
        
        
        
        public IActionResult SearchAjax(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return PartialView("_SearchResult", new List<Food>());
            }

            var result = _context.Foods
                .AsNoTracking()
                .Where(p => p.FoodName.Contains(keyword))
                .OrderByDescending(p => p.FoodId)
                .Take(5)
                .ToList();

            return PartialView("_SearchResult", result);
        }
    }
}