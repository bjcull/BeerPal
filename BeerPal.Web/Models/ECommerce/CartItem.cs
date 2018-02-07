namespace BeerPal.Web.Models.ECommerce
{
    public class CartItem
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}