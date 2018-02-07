using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;

namespace BeerPal.Web.Models.Subscribers
{
    public class IndexVm
    {
        public List<BillingPlan> BillingPlans { get; set; } = new List<BillingPlan>();
        public List<Entities.Subscription> Subscriptions { get; set; } = new List<Entities.Subscription>();
    }
}
