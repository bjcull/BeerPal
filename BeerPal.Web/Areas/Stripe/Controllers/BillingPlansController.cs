using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Data.Entities;
using BeerPal.Web.Seeds;
using BeerPal.Web.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using PayPal.BillingPlans;
using Stripe;

namespace BeerPal.Web.Areas.Stripe.Controllers
{
    [Area("stripe")]
    public class BillingPlansController : Controller
    {        
        private readonly ApplicationDbContext _dbContext;
        private readonly StripeApiFactory _apiFactory;

        public BillingPlansController(ApplicationDbContext dbContext, StripeApiFactory apiFactory)
        {
            _dbContext = dbContext;
            _apiFactory = apiFactory;
        }

        public async Task<IActionResult> Index()
        {
            // Check for database records and add new plans if missing
            var hasMissingPayPalPlans = _dbContext.BillingPlans.Any(x => string.IsNullOrEmpty(x.StripePlanId));
            if (hasMissingPayPalPlans)
            {
                await SeedBillingPlans();
            }

            var client = new StripePlanService(_apiFactory.GetApiKey());

            var list = await client.ListAsync();

            return View(list);
        }

        //public ActionResult Delete(string id)
        //{
        //    var client = _clientFactory.GetClient();
            
        //    var request = new PlanUpdateRequest(id)
        //    var plan = new Plan()
        //    {
        //        id = id
        //    };

        //    plan.Delete(apiContext);

        //    return RedirectToAction("Index");
        //}

        //public ActionResult DeleteAll()
        //{
        //    var apiContext = GetApiContext();

        //    PayPal.BillingPlans.PlanListRequest
        //    var list = PayPal.BillingPlans.Plan.List(apiContext, status: "ACTIVE");

        //    foreach (var plan in list.plans)
        //    {
        //        var deletePlan = new Plan()
        //        {
        //            id = plan.id
        //        };

        //        deletePlan.Delete(apiContext);
        //    }

        //    return RedirectToAction("Index");
        //}

        /// <summary>
        /// Create the default billing plans for this example website
        /// </summary>
        private async Task SeedBillingPlans()
        {
            var client = new StripePlanService(_apiFactory.GetApiKey());
            
            foreach (var plan in BillingPlanSeed.StripePlans())
            {
                // Create Plan
                var planResult = await client.CreateAsync(plan);

                // Add to database record
                var dbPlan = _dbContext.BillingPlans.FirstOrDefault(x =>
                    x.Name == planResult.Name);

                if (dbPlan != null && string.IsNullOrEmpty(dbPlan.StripePlanId))
                {
                    dbPlan.StripePlanId = planResult.Id;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}