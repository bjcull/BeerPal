using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Twitter.Messages;
using Newtonsoft.Json;
using PayPal.Api;

namespace BeerPal.Controllers
{
    public class WebhooksController : Controller
    {
        // GET: Webhooks
        public ActionResult Index()
        {
            var apiContext = GetApiContext();

            var list = Webhook.GetAll(apiContext);

            if (!list.webhooks.Any())
            {
                SeedWebhooks(apiContext);
                list = Webhook.GetAll(apiContext);
            }

            return View(list);
        }

        [HttpPost]
        public ActionResult Delete(string webhookId)
        {
            var apiContext = GetApiContext();

            var webhook = new Webhook()
            {
                id = webhookId
            };

            webhook.Delete(apiContext);

            return RedirectToAction("Index");
        }

        private void SeedWebhooks(APIContext apiContext)
        {
            var callbackUrl = Url.Action("Receive", "WebhookEvents", null, Request.Url.Scheme);

            if (Request.Url.Host == "localhost")
            {
                // Replace with your Ngrok tunnel url
                callbackUrl = "https://f156f219.ngrok.io/WebhookEvents/Receive";
            }

            var everythingWebhook = new Webhook()
            {
                url = callbackUrl,
                event_types = new List<WebhookEventType>
                {
                    new WebhookEventType
                    {
                        name = "PAYMENT.SALE.REFUNDED"
                    },
                    new WebhookEventType
                    {
                        name = "PAYMENT.SALE.REVERSED"
                    },
                    new WebhookEventType
                    {
                        name = "CUSTOMER.DISPUTE.CREATED"
                    },
                    new WebhookEventType
                    {
                        name = "BILLING.SUBSCRIPTION.CANCELLED"
                    },
                    new WebhookEventType
                    {
                        name = "BILLING.SUBSCRIPTION.SUSPENDED"
                    },
                    new WebhookEventType
                    {
                        name = "BILLING.SUBSCRIPTION.RE-ACTIVATED"
                    },
                }
            };
            Webhook.Create(apiContext, everythingWebhook);
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