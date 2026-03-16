using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class Food
    {
        public int FoodId { get; set; }

        [Required]
        public string FoodName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int DiscountPercent { get; set; }   // % giảm giá

        public decimal DiscountPrice { get; set; } // giá sau giảm

        public string Description { get; set; }

        public string? Image { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}