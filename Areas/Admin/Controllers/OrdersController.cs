using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastFood.Data;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : AdminController
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách đơn
        public IActionResult Index()
        {
            var orders = _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // Xem món trong đơn
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Food)
                .FirstOrDefault(o => o.Id == id);

            return View(order);
        }

        // Đổi trạng thái
        public IActionResult UpdateStatus(int id, string status)
        {
            var order = _context.Orders.Find(id);

            order.Status = status;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}