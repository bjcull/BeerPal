using System.Linq;
using BeerPal.Web.Entities;
using BeerPal.Web.Models;
using BeerPal.Web.Models.Subscription;
using Microsoft.AspNetCore.Mvc;
using IndexVm = BeerPal.Web.Models.Home.IndexVm;

namespace BeerPal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
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