namespace BeerPal.Data.Entities
{
    public class BillingPlan : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfBeers { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }       

        public string PayPalPlanId { get; set; }
        public string StripePlanId { get; set; }
        public string BraintreePlanId { get; set; }
    }
}
