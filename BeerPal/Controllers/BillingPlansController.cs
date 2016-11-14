using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace BeerPal.Controllers
{
    public class BillingPlansController : Controller
    {
        public ActionResult Index()
        {
            var apiContext = GetApiContext();

            var list = PayPal.Api.Plan.List(apiContext, status: "ACTIVE");

            if (list == null || !list.plans.Any())
            {
                SeedBillingPlans(apiContext);
                list = PayPal.Api.Plan.List(apiContext, status: "ACTIVE");
            }

            return View(list);
        }

        public ActionResult Delete(string id)
        {
            var apiContext = GetApiContext();

            var plan = new Plan()
            {
                id = id
            };

            plan.Delete(apiContext);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteAll()
        {
            var apiContext = GetApiContext();

            var list = PayPal.Api.Plan.List(apiContext, status: "ACTIVE");

            foreach (var plan in list.plans)
            {
                var deletePlan = new Plan()
                {
                    id = plan.id
                };

                deletePlan.Delete(apiContext);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create the default billing plans for this example website
        /// </summary>
        private void SeedBillingPlans(APIContext apiContext)
        {
            var justBrowsingPlan = new Plan()
            {
                name = "Just Browsing Plan",
                description = "1 great new beer sent to your door each month.",
                type = "infinite",
                payment_definitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        name = "Regular Payments",
                        type = "REGULAR",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = new Currency()
                        {
                            currency = "USD",
                            value = "5.00"
                        },
                        cycles = "0"
                    }
                },
                merchant_preferences = new MerchantPreferences()
                {
                    // The initial payment
                    setup_fee = new Currency()
                    {
                        currency = "USD",
                        value = "5.00"
                    },
                    return_url = Url.Action("Return", "Subscription", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Subscription", null, Request.Url.Scheme)
                }
            };

            var letsDoThisPlan = new Plan()
            {
                name = "Let's Do This Plan",
                description = "A refreshing 6-pack of assorted beers delivered to your door each month.",
                type = "infinite",
                payment_definitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        name = "Regular Payments",
                        type = "REGULAR",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = new Currency()
                        {
                            currency = "USD",
                            value = "24.95"
                        },
                        cycles = "0"
                    }
                },
                merchant_preferences = new MerchantPreferences()
                {
                    // The initial payment
                    setup_fee = new Currency()
                    {
                        currency = "USD",
                        value = "24.95"
                    },
                    return_url = Url.Action("Return", "Subscription", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Subscription", null, Request.Url.Scheme)
                }
            };

            var beardIncludedPlan = new Plan()
            {
                name = "Beard Included Plan",
                description = "A hand picked carton of the most delicious and rare beers placed delicately on your doorstep each month.",
                type = "infinite",
                payment_definitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        name = "Regular Payments",
                        type = "REGULAR",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = new Currency()
                        {
                            currency = "USD",
                            value = "59.95"
                        },
                        cycles = "0"
                    }
                },
                merchant_preferences = new MerchantPreferences()
                {
                    // The initial payment
                    setup_fee = new Currency()
                    {
                        currency = "USD",
                        value = "59.95"
                    },
                    return_url = Url.Action("Return", "Subscription", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Subscription", null, Request.Url.Scheme)
                }
            };

            var hookItToMyVeinsPlan = new Plan()
            {
                name = "Hook It To My Veins Plan",
                description = "Angels whisper sweet nothings into your ears as 48 precious glass bottles are carefully packed into your fridge each month.",
                type = "infinite",
                payment_definitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        name = "Regular Payments",
                        type = "REGULAR",
                        frequency = "MONTH",
                        frequency_interval = "1",
                        amount = new Currency()
                        {
                            currency = "USD",
                            value = "100.00"
                        },
                        cycles = "0"
                    }
                },
                merchant_preferences = new MerchantPreferences()
                {
                    // The initial payment
                    setup_fee = new Currency()
                    {
                        currency = "USD",
                        value = "100.00"
                    },
                    return_url = Url.Action("Return", "Subscription", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Subscription", null, Request.Url.Scheme)
                }
            };

            justBrowsingPlan = Plan.Create(apiContext, justBrowsingPlan);
            letsDoThisPlan = Plan.Create(apiContext, letsDoThisPlan);
            beardIncludedPlan = Plan.Create(apiContext, beardIncludedPlan);
            hookItToMyVeinsPlan = Plan.Create(apiContext, hookItToMyVeinsPlan);

            var updateStatePatchRequest = new PatchRequest()
            {
                new Patch()
                {
                    op = "replace",
                    path = "/",
                    value = new Plan
                    {
                        state = "ACTIVE"
                    }
                }
            };

            justBrowsingPlan.Update(apiContext, updateStatePatchRequest);
            letsDoThisPlan.Update(apiContext, updateStatePatchRequest);
            beardIncludedPlan.Update(apiContext, updateStatePatchRequest);
            hookItToMyVeinsPlan.Update(apiContext, updateStatePatchRequest);
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