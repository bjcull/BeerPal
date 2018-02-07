using System;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;
using BeerPal.Web.Models.Subscription;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PayPal.BillingAgreements;
using Stripe;
using PurchaseVm = BeerPal.Web.Areas.Stripe.Models.Subscription.PurchaseVm;

namespace BeerPal.Web.Areas.Stripe.Controllers
{
    [Area("stripe")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StripeApiFactory _apiFactory;

        public SubscriptionController(ApplicationDbContext dbContext, StripeApiFactory apiFactory)
        {
            _dbContext = dbContext;
            _apiFactory = apiFactory;
        }

        public ActionResult Index()
        {
            var model = new IndexVm()
            {
                Plans = _dbContext.BillingPlans.ToList()
            };

            return View(model);
        }

        public ActionResult Purchase(string id)
        {
            var model = new PurchaseVm()
            {
                Plan = _dbContext.BillingPlans.FirstOrDefault(x => x.StripePlanId == id),
                PublishableKey = _apiFactory.GetPublishableKey()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseVm model)
        {
            var plan = _dbContext.BillingPlans.FirstOrDefault(x => x.StripePlanId == model.Plan.StripePlanId);

            if (ModelState.IsValid && plan != null)
            {
                var customerOptions = new StripeCustomerCreateOptions()
                {
                    PlanId = plan.StripePlanId,
                    SourceToken = model.StripeToken,
                    Email = model.Email                    
                };
                var client = new StripeCustomerService(_apiFactory.GetApiKey());
                var customer = await client.CreateAsync(customerOptions);

                var subscription = new Subscription()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    StartDate = DateTime.UtcNow,
                    StripePlanId = plan.StripePlanId,
                    StripeCustomerId = customer.Id,
                    StripeSubscriptionId = customer.Subscriptions?.FirstOrDefault()?.Id
                };
                _dbContext.Subscriptions.Add(subscription);
                _dbContext.SaveChanges();

                return RedirectToAction("ThankYou");
            }

            model.Plan = plan;
            return View(model);
        }

        public ActionResult Cancel()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}