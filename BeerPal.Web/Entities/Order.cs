using System;
using System.Collections.Generic;

namespace BeerPal.Web.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
        public int Tax { get; set; }
        public int Subtotal { get; set; }
        public int Shipping { get; set; }
        public string PayPalReference { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}