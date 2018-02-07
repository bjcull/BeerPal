using System.Collections.Generic;
using BeerPal.Web.Entities;
using BeerPal.Web.Models.Subscription;

namespace BeerPal.Web.Models.Home
{
    public class IndexVm
    {
        public List<Beer> Beers { get; set; }
        public List<BillingPlan> Plans { get; set; }

        public IndexVm()
        {
            Beers = new List<Beer>();
            Plans = new List<BillingPlan>();
        }
    }
}