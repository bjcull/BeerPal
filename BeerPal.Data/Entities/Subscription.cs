using System;

namespace BeerPal.Data.Entities
{
    public class Subscription : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }

        public string PayPalPlanId { get; set; }
        public string PayPalAgreementToken { get; set; }
        public string PayPalAgreementId { get; set; }

        public string StripePlanId { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }

        public string BraintreePlanId { get; set; }
        public string BraintreeCustomerId { get; set; }
        public string BraintreeSubscriptionId { get; set; }
    }
}