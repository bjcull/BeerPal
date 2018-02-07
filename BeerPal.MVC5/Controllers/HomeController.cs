using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerPal.Models;
using BeerPal.Models.Subscription;
using Microsoft.AspNet.Identity.Owin;
using IndexVm = BeerPal.Models.Home.IndexVm;

namespace BeerPal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext => HttpContext.GetOwinContext().Get<ApplicationDbContext>();

        public ActionResult Index()
        {
            var model = new IndexVm()
            {
                Beers = _dbContext.Beers.ToList(),
                Plans = Plan.Plans
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