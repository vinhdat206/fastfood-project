using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastFood.Data;
using FastFood.Models;

namespace FastFood.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lịch sử đơn hàng
        public IActionResult History()
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Food)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // Chi tiết đơn hàng
        public IActionResult Detail(int id)
        {
            var details = _context.OrderDetails
                .Include(d => d.Food)
                .Where(d => d.OrderId == id)
                .ToList();

            return View(details);
        }
    }
}