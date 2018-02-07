using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerPal.Entities;
using BeerPal.Models.Subscription;

namespace BeerPal.Models.Home
{
    public class IndexVm
    {
        public List<Beer> Beers { get; set; }
        public List<Plan> Plans { get; set; }

        public IndexVm()
        {
            Beers = new List<Beer>();
            Plans = new List<Plan>();
        }
    }
}