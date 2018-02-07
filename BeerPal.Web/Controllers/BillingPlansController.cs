using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;
using BeerPal.Web.Seeds;
using BeerPal.Web.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using PayPal.BillingPlans;

namespace BeerPal.Web.Controllers
{
    public class BillingPlansController : Controller
    {
        private readonly PayPalHttpClientFactory _clientFactory;
        private readonly ApplicationDbContext _dbContext;

        public BillingPlansController(PayPalHttpClientFactory clientFactory, ApplicationDbContext dbContext)
        {
            _clientFactory = clientFactory;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            // Check for database records and add new plans if missing
            var hasMissingPayPalPlans = _dbContext.BillingPlans.Any(x => string.IsNullOrEmpty(x.PayPalPlanId));
            if (hasMissingPayPalPlans)
            {
                await SeedBillingPlans();
            }

            var client = _clientFactory.GetClient();

            var request = new PlanListRequest()
                .Status("ACTIVE")
                .PageSize("20");

            var result = await client.Execute(request);
            var list = result.Result<PlanList>();

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
            var client = _clientFactory.GetClient();

            foreach (var plan in BillingPlanSeed.PayPalPlans(
                Url.Action("Return", "Subscription", null, Request.GetUri().Scheme),
                Url.Action("Cancel", "Subscription", null, Request.GetUri().Scheme)))
            {
                // Create Plan
                var request = new PlanCreateRequest().RequestBody(plan);
                var result = await client.Execute(request);
                var obj = result.Result<Plan>();

                // Activate Plan
                var activateRequest = new PlanUpdateRequest<Plan>(obj.Id)
                    .RequestBody(GetActivatePlanBody());
                await client.Execute(activateRequest);

                // Add to database record
                var dbPlan = _dbContext.BillingPlans.FirstOrDefault(x =>
                    x.Name == obj.Name);

                if (dbPlan != null && string.IsNullOrEmpty(dbPlan.PayPalPlanId))
                {
                    dbPlan.PayPalPlanId = obj.Id;
                    await _dbContext.SaveChangesAsync();
                }
            }

            //// Create plans
            //var justBrowsingPlanRequest = new PlanCreateRequest().RequestBody(justBrowsingPlan);
            //var justBrowsingPlanResult = await client.Execute(justBrowsingPlanRequest);
            //var justBrowsingPlanObject = justBrowsingPlanResult.Result<Plan>();

            //var letsDoThisPlanRequest = new PlanCreateRequest().RequestBody(letsDoThisPlan);
            //var letsDoThisPlanResult = await client.Execute(letsDoThisPlanRequest);
            //var letsDoThisPlanObject = letsDoThisPlanResult.Result<Plan>();

            //var beardIncludedPlanRequest = new PlanCreateRequest().RequestBody(beardIncludedPlan);
            //var beardIncludedPlanResult = await client.Execute(beardIncludedPlanRequest);
            //var beardIncludedPlanObject = beardIncludedPlanResult.Result<Plan>();

            //var hookItToMyVeinsPlanRequest = new PlanCreateRequest().RequestBody(hookItToMyVeinsPlan);
            //var hookItToMyVeinsPlanResult = await client.Execute(hookItToMyVeinsPlanRequest);
            //var hookItToMyVeinsPlanObject = hookItToMyVeinsPlanResult.Result<Plan>();

            //// Activate plans
            //var activateJustBrowsingPlanRequest = new PlanUpdateRequest<Plan>(justBrowsingPlanObject.Id)
            //    .RequestBody(GetActivatePlanBody());
            //await client.Execute(activateJustBrowsingPlanRequest);

            //var activateletsDoThisPlanRequest = new PlanUpdateRequest<Plan>(letsDoThisPlanObject.Id)
            //    .RequestBody(GetActivatePlanBody());
            //await client.Execute(activateletsDoThisPlanRequest);

            //var activateBeardIncludedPlanRequest = new PlanUpdateRequest<Plan>(beardIncludedPlanObject.Id)
            //    .RequestBody(GetActivatePlanBody());
            //await client.Execute(activateBeardIncludedPlanRequest);

            //var activateHookItToMyVeinsPlanRequest = new PlanUpdateRequest<Plan>(hookItToMyVeinsPlanObject.Id)
            //    .RequestBody(GetActivatePlanBody());
            //await client.Execute(activateHookItToMyVeinsPlanRequest);
        }

        private static List<JsonPatch<Plan>> GetActivatePlanBody()
        {
            return new List<JsonPatch<Plan>>()
            {
                new JsonPatch<Plan>()
                {
                    Op = "replace",
                    Path = "/",
                    Value = new Plan()
                    {
                        State = "ACTIVE"
                    }
                }
            };            
        }
    }
}