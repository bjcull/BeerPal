using System.Collections.Generic;
using BeerPal.Web.Entities;

namespace BeerPal.Web.Areas.Braintree.Models.Subscribers
{
    public class IndexVm
    {
        public List<BillingPlan> BillingPlans { get; set; } = new List<BillingPlan>();
        public List<Entities.Subscription> Subscriptions { get; set; } = new List<Entities.Subscription>();
    }
}
