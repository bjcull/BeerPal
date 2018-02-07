using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PayPal.Api;

namespace BeerPal.Extras
{
    public class SimulatableWebhook : Webhook
    {
        public WebhookEvent SimulateEvent(string eventType, string webhookId)
        {
            var apiContext = GetApiContext();

            if (apiContext.HTTPHeaders == null)
                apiContext.HTTPHeaders = new Dictionary<string, string>();
            apiContext.HTTPHeaders["Content-Type"] = "application/json";
            apiContext.SdkVersion = new SDKVersion();

            string resource = "v1/notifications/simulate-event";

            var data = new
            {
                webhook_id = webhookId,
                event_type = eventType
            };

            var webhookEvent = PayPalResource.ConfigureAndExecute<WebhookEvent>(apiContext, PayPalResource.HttpMethod.POST, resource, JsonConvert.SerializeObject(data), "", true);

            return webhookEvent;            
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