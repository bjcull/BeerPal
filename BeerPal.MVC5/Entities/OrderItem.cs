using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPal.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}