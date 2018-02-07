using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Areas.PayPal.Models.Subscribers;
using BeerPal.Web.Entities;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Mvc;
using PayPal.BillingAgreements;

namespace BeerPal.Web.Areas.PayPal.Controllers
{
    [Area("paypal")]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PayPalHttpClientFactory _clientFactory;

        public SubscribersController(ApplicationDbContext dbContext, PayPalHttpClientFactory clientFactory)
        {
            _dbContext = dbContext;
            _clientFactory = clientFactory;
        }

        public ActionResult Index()
        {
            var model = new IndexVm()
            {
                BillingPlans = _dbContext.BillingPlans.ToList(),
                Subscriptions = _dbContext.Subscriptions
                    .Where(x => !string.IsNullOrEmpty(x.PayPalAgreementId))
                    .OrderByDescending(x => x.StartDate).Take(50).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var client = _clientFactory.GetClient();

            var request = new AgreementGetRequest(id);
            var result = await client.Execute(request);
            var agreement = result.Result<Agreement>();

            return View(agreement);
        }

        public async Task<IActionResult> Suspend(string id)
        {
            var client = _clientFactory.GetClient();

            var request = new AgreementSuspendRequest(id).RequestBody(new AgreementStateDescriptor()
            {
                Note = "Suspended"
            });                
            await client.Execute(request);           

            return RedirectToAction("Details", new {id = id});
        }

        public async Task<IActionResult> Reactivate(string id)
        {
            var client = _clientFactory.GetClient();

            var request = new AgreementReActivateRequest(id).RequestBody(new AgreementStateDescriptor()
            {
                Note = "Reactivated"
            });                
            await client.Execute(request);  

            return RedirectToAction("Details", new {id = id});
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var client = _clientFactory.GetClient();

            var request = new AgreementCancelRequest(id).RequestBody(new AgreementStateDescriptor()
            {
                Note = "Cancelled"
            });                
            await client.Execute(request);        

            return RedirectToAction("Details", new {id = id});
        }
    }
}