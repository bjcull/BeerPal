using System.ComponentModel.DataAnnotations;
using BeerPal.Web.Entities;

namespace BeerPal.Web.Models.Subscription
{
    public class PurchaseVm
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        public BillingPlan Plan { get; set; }
    }
}