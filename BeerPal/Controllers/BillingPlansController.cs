using System;
using System.Collections.Generic;
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

            var list = PayPal.Api.Plan.List(apiContext);

            if (list == null || !list.plans.Any())
            {
                SeedBillingPlans(apiContext);
                list = PayPal.Api.Plan.List(apiContext);
            }

            return View(list);
        }

        /// <summary>
        /// Create the default web experience profiles for this example website
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
                    return_url = Url.Action("Return", "Subscription", null, Request.Url.Scheme),
                    cancel_url = Url.Action("Cancel", "Subscription", null, Request.Url.Scheme)
                }
            };

            justBrowsingPlan.Create(apiContext);
            letsDoThisPlan.Create(apiContext);
            beardIncludedPlan.Create(apiContext);
            hookItToMyVeinsPlan.Create(apiContext);
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