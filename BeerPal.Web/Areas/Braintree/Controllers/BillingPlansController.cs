using System.Linq;
using System.Threading.Tasks;
using BeerPal.Data.Entities;
using BeerPal.Web.Seeds;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BeerPal.Web.Areas.Braintree.Controllers
{
    [Area("braintree")]
    public class BillingPlansController : Controller
    {        
        private readonly ApplicationDbContext _dbContext;
        private readonly BraintreeApiFactory _apiFactory;

        public BillingPlansController(ApplicationDbContext dbContext, BraintreeApiFactory apiFactory)
        {
            _dbContext = dbContext;
            _apiFactory = apiFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _apiFactory.GetClient();

            var list = await client.Plan.AllAsync();

            return View(list);
        }        
    }
}