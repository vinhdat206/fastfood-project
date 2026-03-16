using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int FoodId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? Note { get; set; }

        public Food Food { get; set; }   // thêm dòng này
    }
}