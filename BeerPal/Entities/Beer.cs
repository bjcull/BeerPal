using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPal.Entities
{
    public class Beer : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}