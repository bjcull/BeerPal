using System.Collections.Generic;

namespace BeerPal.Web.Models.ECommerce
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}