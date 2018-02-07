using System.Collections.Generic;

namespace BeerPal.Web.Models.Subscription
{
    public class Plan
    {
        public string PayPalPlanId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfBeers { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
    }
}