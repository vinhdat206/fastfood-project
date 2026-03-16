using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using System.Linq;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : AdminController
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            ViewBag.Users = _context.Users.Count();

            ViewBag.Orders = _context.Orders.Count();

            ViewBag.Products = _context.Foods.Count();

            ViewBag.Revenue = _context.Orders
                .Sum(x => (decimal?)x.TotalAmount) ?? 0;


            // doanh thu theo tháng

            var revenueByMonth = _context.Orders
                .GroupBy(o => o.CreatedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(x => x.Month)
                .ToList();


            ViewBag.ChartLabels = revenueByMonth
                .Select(x => "Tháng " + x.Month)
                .ToList();

            ViewBag.ChartData = revenueByMonth
                .Select(x => x.Total)
                .ToList();


            ViewBag.RecentOrders = _context.Orders
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();

            return View();
        }
    }
}