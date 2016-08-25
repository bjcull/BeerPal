using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerPal.Entities;

namespace BeerPal.Models.Home
{
    public class IndexVm
    {
        public List<Beer> Beers { get; set; }
    }
}