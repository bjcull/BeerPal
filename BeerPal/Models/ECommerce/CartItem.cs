using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPal.Models.ECommerce
{
    public class CartItem
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}