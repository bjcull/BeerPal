using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerPal.Entities;
using BeerPal.Models;
using BeerPal.Models.Subscription;
using Microsoft.AspNet.Identity.Owin;
using PayPal.Api;
using Plan = BeerPal.Models.Subscription.Plan;

namespace BeerPal.Controllers
{
    public class SubscriptionController : Controller
    {
        private ApplicationDbContext _dbContext => HttpContext.GetOwinContext().Get<ApplicationDbContext>();

        public ActionResult Index()
        {
            var model = new IndexVm()
            {
                Plans = Plan.Plans
            };

            return View(model);
        }

        public ActionResult Purchase(string id)
        {
            var model = new PurchaseVm()
            {
                Plan = Plan.Plans.FirstOrDefault(x => x.PayPalPlanId == id)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Purchase(PurchaseVm model)
        {
            var plan = Plan.Plans.FirstOrDefault(x => x.PayPalPlanId == model.Plan.PayPalPlanId);

            if (ModelState.IsValid)
            {
                // Since we take an Initial Payment (instant payment), the start date of the recurring payments will be next month.
                var startDate = DateTime.UtcNow.AddMonths(1);

                var apiContext = GetApiContext();

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
                    name = plan.Name,
                    description = $"{plan.NumberOfBeers} beer(s) delivered for ${(plan.Price/100M).ToString("0.00")} each month.",
                    start_date = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    plan = new PayPal.Api.Plan()
                    {
                        id = plan.PayPalPlanId
                    },
                    payer = new Payer()
                    {
                        payment_method = "paypal"
                    }
                };

                // Send the agreement to PayPal
                var createdAgreement = agreement.Create(apiContext);

                // Save the token so we can match the returned request to our subscription.
                subscription.PayPalAgreementToken = createdAgreement.token;
                _dbContext.SaveChanges();

                // Find the Approval URL to send our user to
                var approvalUrl =
                    createdAgreement.links.FirstOrDefault(
                        x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                // Send the user to PayPal to approve the payment
                return Redirect(approvalUrl.href);
            }

            model.Plan = plan;
            return View(model);
        }

        public ActionResult Return(string token)
        {
            var subscription = _dbContext.Subscriptions.FirstOrDefault(x => x.PayPalAgreementToken == token);

            var apiContext = GetApiContext();

            var agreement = new Agreement()
            {
                token = token
            };

            var executedAgreement = agreement.Execute(apiContext);

            // Save the PayPal agreement in our subscription so we can look it up later.
            subscription.PayPalAgreementId = executedAgreement.id;
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

        private APIContext GetApiContext()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }
    }
}