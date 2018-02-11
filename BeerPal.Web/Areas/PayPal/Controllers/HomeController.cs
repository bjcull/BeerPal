using System.Linq;
using BeerPal.Data.Entities;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Mvc;
using IndexVm = BeerPal.Web.Models.Home.IndexVm;

namespace BeerPal.Web.Areas.PayPal.Controllers
{
    [Area("paypal")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly GatewaySwitchService _gatewaySwitchService;

        public HomeController(ApplicationDbContext dbContext, GatewaySwitchService gatewaySwitchService)
        {
            _dbContext = dbContext;
            _gatewaySwitchService = gatewaySwitchService;
        }

        public ActionResult Index()
        {
            //Set current gateway
            _gatewaySwitchService.SetCurrentGateway(HttpContext.Session, Gateway.PayPal);

            var model = new IndexVm()
            {
                Beers = _dbContext.Beers.ToList(),
                Plans = _dbContext.BillingPlans.ToList()
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}