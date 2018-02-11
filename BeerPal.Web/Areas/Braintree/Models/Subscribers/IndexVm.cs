using System.Collections.Generic;
using BeerPal.Data.Entities;

namespace BeerPal.Web.Areas.Braintree.Models.Subscribers
{
    public class IndexVm
    {
        public List<BillingPlan> BillingPlans { get; set; } = new List<BillingPlan>();
        public List<Data.Entities.Subscription> Subscriptions { get; set; } = new List<Data.Entities.Subscription>();
    }
}
