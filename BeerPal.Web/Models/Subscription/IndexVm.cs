using System.Collections.Generic;
using BeerPal.Web.Entities;

namespace BeerPal.Web.Models.Subscription
{
    public class IndexVm
    {
        public List<BillingPlan> Plans { get; set; } = new List<BillingPlan>();
    }
}