using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using FastFood.Models;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodsController : AdminController
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var foods = _context.Foods.ToList();
            return View(foods);
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Food food, IFormFile ImageFile)
        {
            if (food.DiscountPercent > 0)
            {
                food.DiscountPrice = food.Price - (food.Price * food.DiscountPercent / 100);
            }
            else
            {
                food.DiscountPrice = food.Price;
            }

            // upload ảnh
            if (ImageFile != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/images/foods", ImageFile.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                food.Image = "/images/foods/" + ImageFile.FileName;
            }

            _context.Foods.Add(food);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var food = _context.Foods.Find(id);
            return View(food);
        }

        [HttpPost]
        public IActionResult Edit(Food food)
        {
            _context.Foods.Update(food);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var food = _context.Foods.Find(id);
            return View(food);
        }

        [HttpPost]
        public IActionResult Delete(int id, Food food)
        {
            var item = _context.Foods.Find(id);

            _context.Foods.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        
        public IActionResult Details(int id)
        {
            var food = _context.Foods.Find(id);

            if(food == null)
                return NotFound();

            return View(food);
        }
    }
}