using System.ComponentModel.DataAnnotations;
using BeerPal.Data.Entities;

namespace BeerPal.Web.Areas.Stripe.Models.Subscription
{
    public class PurchaseVm
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required] 
        public string StripeToken { get; set; }

        public BillingPlan Plan { get; set; }
        public string PublishableKey { get; set; }
    }
}