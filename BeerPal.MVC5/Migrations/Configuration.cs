using System.Collections.Generic;
using BeerPal.Entities;

namespace BeerPal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BeerPal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BeerPal.Models.ApplicationDbContext context)
        {
            if (!context.Beers.Any())
            {
                var beers = new List<Beer>()
                {
                    new Beer() { Name = "Raspberry Double IPA", Price = 1200 },
                    new Beer() { Name = "Goddess Helles Bock", Price = 900 },
                    new Beer() { Name = "Downward Dog Pumpkin Ale", Price = 1400 },
                    new Beer() { Name = "Jalapeño Half Moon Dunkel", Price = 1000 },
                    new Beer() { Name = "Mud Guard Pumpkin Ale", Price = 1000 },
                    new Beer() { Name = "Molten Mocha IPA", Price = 1100 },
                    new Beer() { Name = "Big Toe Saison", Price = 700 },
                    new Beer() { Name = "Nutmeg Doppelgänger Kölsch", Price = 900 },
                };
                context.Beers.AddRange(beers);
                context.SaveChanges();
            }
        }
    }
}
