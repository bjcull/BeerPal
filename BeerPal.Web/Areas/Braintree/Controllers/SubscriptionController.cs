using System;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Data.Entities;
using BeerPal.Web.Areas.Braintree.Models.Subscription;
using BeerPal.Web.Models.Subscription;
using BeerPal.Web.Services;
using Braintree;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Subscription = BeerPal.Data.Entities.Subscription;

namespace BeerPal.Web.Areas.Braintree.Controllers
{
    [Area("braintree")]
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BraintreeApiFactory _apiFactory;

        public SubscriptionController(ApplicationDbContext dbContext, BraintreeApiFactory apiFactory)
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

        public async Task<IActionResult> Purchase(string id)
        {
            var client = _apiFactory.GetClient();

            var clientToken = await client.ClientToken.GenerateAsync();

            var model = new PurchaseVm()
            {
                Plan = _dbContext.BillingPlans.FirstOrDefault(x => x.BraintreePlanId == id),
                ClientAuthToken = clientToken
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseVm model)
        {
            var plan = _dbContext.BillingPlans.FirstOrDefault(x => x.BraintreePlanId == model.Plan.BraintreePlanId);
            var client = _apiFactory.GetClient();

            if (ModelState.IsValid && plan != null)
            {
                var request = new CustomerRequest()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PaymentMethodNonce = model.BraintreeNonce
                };
                var customer = await client.Customer.CreateAsync(request);

                var subscriptionRequest = new SubscriptionRequest()
                {
                    PlanId = plan.BraintreePlanId,
                    PaymentMethodToken = customer.Target.PaymentMethods.First().Token
                };
                var braintreeSubscription = await client.Subscription.CreateAsync(subscriptionRequest);

                var subscription = new Subscription()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    StartDate = DateTime.UtcNow,
                    BraintreePlanId = plan.BraintreePlanId,
                    BraintreeCustomerId = customer.Target.Id,
                    BraintreeSubscriptionId = braintreeSubscription.Target.Id
                };
                _dbContext.Subscriptions.Add(subscription);
                _dbContext.SaveChanges();

                return RedirectToAction("ThankYou");
            }

            var clientToken = await client.ClientToken.GenerateAsync();
            model.ClientAuthToken = clientToken;
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