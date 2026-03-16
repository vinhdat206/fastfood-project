using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using System.Linq;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactsController : AdminController
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var contacts = _context.Contacts
                .OrderByDescending(c => c.Id)
                .ToList();

            return View(contacts);
        }
    }
}