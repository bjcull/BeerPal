using System.ComponentModel.DataAnnotations;
using BeerPal.Web.Entities;

namespace BeerPal.Web.Areas.Braintree.Models.Subscription
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
        public string BraintreeNonce { get; set; }

        public BillingPlan Plan { get; set; }
        public string ClientAuthToken { get; set; }
    }
}