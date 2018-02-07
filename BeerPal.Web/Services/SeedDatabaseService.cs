using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;
using BeerPal.Web.Models;
using BeerPal.Web.Seeds;

namespace BeerPal.Web.Services
{
    public class SeedDatabaseService
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedDatabaseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedDatabase()
        {
            // Check for Billing Plans
            if (!_dbContext.BillingPlans.Any())
            {
                foreach (var plan in BillingPlanSeed.Plans)
                {
                    _dbContext.BillingPlans.Add(plan);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
