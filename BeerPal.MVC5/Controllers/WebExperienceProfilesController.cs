using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace BeerPal.Controllers
{
    public class WebExperienceProfilesController : Controller
    {
        public ActionResult Index()
        {
            var apiContext = GetApiContext();

            var list = WebProfile.GetList(apiContext);

            if (!list.Any())
            {
                SeedWebProfiles(apiContext);
                list = WebProfile.GetList(apiContext);
            }

            return View(list);
        }

        /// <summary>
        /// Create the default web experience profiles for this example website
        /// </summary>
        private void SeedWebProfiles(APIContext apiContext)
        {
            var digitalGoods = new WebProfile()
            {
                name = "DigitalGoods",
                input_fields = new InputFields()
                {
                    no_shipping = 1
                }
            };
            WebProfile.Create(apiContext, digitalGoods);
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