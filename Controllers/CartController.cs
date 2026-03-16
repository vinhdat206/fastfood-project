using Microsoft.AspNetCore.Mvc;
using FastFood.Data;
using FastFood.Models;
using Newtonsoft.Json;

namespace FastFood.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy cart từ session
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");

            if (cartJson == null)
                return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        // Lưu cart vào session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart",
                JsonConvert.SerializeObject(cart));
        }

        // Trang giỏ hàng
        public IActionResult Index()
        {
            var cart = GetCart();

            ViewBag.CartCount = cart.Sum(x => x.Quantity);

            return View(cart);
        }

        // Thêm vào giỏ
        public IActionResult AddToCart(int id, int qty, string note)
        {
            // chưa login → chuyển sang login
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var food = _context.Foods.Find(id);
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.FoodId == id);

            if (item != null)
            {
                item.Quantity += qty;
            }
            else
            {
                cart.Add(new CartItem
                {
                    FoodId = food.FoodId,
                    FoodName = food.FoodName,
                    Price = food.Price,
                    Quantity = qty,
                    Image = food.Image,
                    Note = note
                });
            }

            SaveCart(cart);
            return Ok(); // hoặc quay lại trang trước
        }
        
        public int GetCartCount()
        {
            var cart = GetCart();
            return cart.Sum(x => x.Quantity);
        }

        // Tăng số lượng
        public IActionResult Increase(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.FoodId == id);

            if (item != null)
                item.Quantity++;

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // Giảm số lượng
        public IActionResult Decrease(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.FoodId == id);

            if (item != null && item.Quantity > 1)
                item.Quantity--;

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm
        public IActionResult Remove(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.FoodId == id);

            if (item != null)
                cart.Remove(item);

            SaveCart(cart);

            return RedirectToAction("Index");
        }

       

        // Đặt hàng
        
        //Checkout
        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = GetCart();
            if (cart == null || cart.Count == 0)
                return RedirectToAction("Index");

            return View(cart);
        }
        
        [HttpPost]
        public IActionResult Checkout(string name, string phone, string address)
        {
            // chưa login → chuyển sang login
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = GetCart();
            if (cart == null || cart.Count == 0)
                return RedirectToAction("Index");

            // tạo đơn hàng
            var order = new Order
            {
                CustomerName = name,
                Phone = phone,
                Address = address,
                OrderDate = DateTime.Now,
                Status = "Đang xử lý",
                TotalAmount = cart.Sum(x => x.Price * x.Quantity)
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    OrderId = order.Id,
                    FoodId = item.FoodId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Note = item.Note
                };

                _context.OrderDetails.Add(detail);
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success");
        }
        


        public IActionResult Success()
        {
            return View();
        }
    }
}