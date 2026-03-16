using System;
using System.Collections.Generic;

namespace FastFood.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = "Pending";
        
        public DateTime CreatedAt { get; set; }
        

        public List<OrderDetail> OrderDetails { get; set; }
    }
}