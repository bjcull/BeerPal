using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerPal.Entities;

namespace BeerPal.Models.ECommerce
{
    public class IndexVm
    {
        public List<Beer> Beers { get; set; }

        public IndexVm()
        {
            Beers = new List<Beer>();
        }
    }
}