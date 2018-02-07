using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Areas.Stripe.Models.Subscribers;
using BeerPal.Web.Entities;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Mvc;
using PayPal.BillingAgreements;
using Stripe;

namespace BeerPal.Web.Areas.Stripe.Controllers
{
    [Area("stripe")]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StripeApiFactory _apiFactory;
        public SubscribersController(ApplicationDbContext dbContext, StripeApiFactory apiFactory)
        {
            _dbContext = dbContext;
            _apiFactory = apiFactory;
        }

        public ActionResult Index()
        {
            var model = new IndexVm()
            {
                BillingPlans = _dbContext.BillingPlans.ToList(),
                Subscriptions = _dbContext.Subscriptions
                    .Where(x => !string.IsNullOrEmpty(x.StripeSubscriptionId))
                    .OrderByDescending(x => x.StartDate).Take(50).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var client = new StripeSubscriptionService(_apiFactory.GetApiKey());

            var agreement = await client.GetAsync(id);

            return View(agreement);
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var client = new StripeSubscriptionService(_apiFactory.GetApiKey());

            await client.CancelAsync(id);

            return RedirectToAction("Details", new {id = id});
        }
    }
}