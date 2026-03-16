namespace FastFood.Models
{
    public class CartItem
    {
        public int FoodId { get; set; }

        public string FoodName { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Note { get; set; }
    }
}