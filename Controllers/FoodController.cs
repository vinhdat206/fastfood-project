using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FastFood.Data;
using FastFood.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FastFood.Controllers
{
    public class FoodController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= MENU =================
        public async Task<IActionResult> Menu()
        {
            var foods = await _context.Foods
                .Include(f => f.Category)
                .ToListAsync();

            return View(foods);
        }

        // ================= DETAILS =================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.FoodId == id);

            if (food == null) return NotFound();

            return View(food);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Food food, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                    var categoryName = _context.Categories
                        .Where(c => c.CategoryId == food.CategoryId)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault()
                        .ToLower();

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(),
                                                  "wwwroot/images/foods",
                                                  categoryName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var fullPath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    food.Image = categoryName + "/" + fileName;
                }

                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Menu));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", food.CategoryId);
            return View(food);
        }

        // ================= EDIT =================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var food = await _context.Foods.FindAsync(id);
            if (food == null) return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", food.CategoryId);
            return View(food);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Food food, IFormFile ImageFile)
        {
            if (id != food.FoodId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null)
                    {
                        // 🔥 xóa ảnh cũ
                        if (!string.IsNullOrEmpty(food.Image))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot/images/foods", food.Image);

                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                        var categoryName = _context.Categories
                            .Where(c => c.CategoryId == food.CategoryId)
                            .Select(c => c.CategoryName)
                            .FirstOrDefault()
                            .ToLower();

                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(),
                                                      "wwwroot/images/foods",
                                                      categoryName);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        var fullPath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        food.Image = categoryName + "/" + fileName;
                    }

                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Foods.Any(e => e.FoodId == food.FoodId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Menu));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", food.CategoryId);
            return View(food);
        }

        // ================= DELETE =================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var food = await _context.Foods
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.FoodId == id);

            if (food == null) return NotFound();

            return View(food);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food != null)
            {
                if (!string.IsNullOrEmpty(food.Image))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/images/foods", food.Image);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                _context.Foods.Remove(food);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Menu));
        }
    }
}