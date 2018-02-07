using System;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;
using BeerPal.Web.Models;
using BeerPal.Web.Models.Subscription;
using BeerPal.Web.Services;
using BraintreeHttp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PayPal.BillingAgreements;

namespace BeerPal.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PayPalHttpClientFactory _clientFactory;

        public SubscriptionController(ApplicationDbContext dbContext, PayPalHttpClientFactory clientFactory)
        {
            _dbContext = dbContext;
            _clientFactory = clientFactory;
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
                Plan = _dbContext.BillingPlans.FirstOrDefault(x => x.PayPalPlanId == id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(PurchaseVm model)
        {
            var plan = _dbContext.BillingPlans.FirstOrDefault(x => x.PayPalPlanId == model.Plan.PayPalPlanId);

            if (ModelState.IsValid && plan != null)
            {
                // Since we take an Initial Payment (instant payment), the start date of the recurring payments will be next month.
                var startDate = DateTime.UtcNow.AddMonths(1);

                var subscription = new Subscription()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    StartDate = startDate,
                    PayPalPlanId = plan.PayPalPlanId
                };
                _dbContext.Subscriptions.Add(subscription);
                _dbContext.SaveChanges();

                var agreement = new Agreement()
                {
                    Name = plan.Name,
                    Description = $"{plan.NumberOfBeers} beer(s) delivered for ${(plan.Price/100M).ToString("0.00")} each month.",
                    StartDate = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Plan = new PayPal.BillingAgreements.Plan()
                    {
                        Id = plan.PayPalPlanId
                    },
                    Payer = new Payer()
                    {
                        PaymentMethod = "paypal"
                    }
                };

                // Send the agreement to PayPal
                var client = _clientFactory.GetClient();
                var request = new AgreementCreateRequest()
                    .RequestBody(agreement);
                var result = await client.Execute(request);
                var createdAgreement = result.Result<Agreement>();
                
                // Find the Approval URL to send our user to (also contains the token)
                var approvalUrl =
                    createdAgreement.Links.FirstOrDefault(
                        x => x.Rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                var token = QueryHelpers.ParseQuery(approvalUrl?.Href)["token"].First();
                
                // Save the token so we can match the returned request to our subscription.
                subscription.PayPalAgreementToken = token;
                _dbContext.SaveChanges();

                // Send the user to PayPal to approve the payment
                return Redirect(approvalUrl.Href);
            }

            model.Plan = plan;
            return View(model);
        }

        public async Task<IActionResult> Return(string token)
        {
            var subscription = _dbContext.Subscriptions.FirstOrDefault(x => x.PayPalAgreementToken == token);

            var client = _clientFactory.GetClient();

            var request = new AgreementExecuteRequest(token);
            request.Body = "{}"; // Bug: Stupid hack workaround for a bug. Lost an hour to this.
            var result = await client.Execute(request);

            var executedAgreement = result.Result<Agreement>();

            // Save the PayPal agreement in our subscription so we can look it up later.
            subscription.PayPalAgreementId = executedAgreement.Id;
            _dbContext.SaveChanges();

            return RedirectToAction("Thankyou");
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