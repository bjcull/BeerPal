using System.Linq;
using System.Threading.Tasks;
using BeerPal.Data.Entities;
using BeerPal.Web.Areas.Braintree.Models.Subscribers;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BeerPal.Web.Areas.Braintree.Controllers
{
    [Area("braintree")]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly BraintreeApiFactory _apiFactory;
        public SubscribersController(ApplicationDbContext dbContext, BraintreeApiFactory apiFactory)
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
                    .Where(x => !string.IsNullOrEmpty(x.BraintreeSubscriptionId))
                    .OrderByDescending(x => x.StartDate).Take(50).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {            
            var client = _apiFactory.GetClient();

            var agreement = await client.Subscription.FindAsync(id);            

            return View(agreement);
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var client = _apiFactory.GetClient();

            await client.Subscription.CancelAsync(id);            

            return RedirectToAction("Details", new {id = id});
        }
    }
}