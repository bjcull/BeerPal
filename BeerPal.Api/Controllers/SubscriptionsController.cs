using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeerPal.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class SubscriptionsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var subscriptions = await _dbContext.Subscriptions.ToListAsync();

            return Json(subscriptions);
        }
    }
}