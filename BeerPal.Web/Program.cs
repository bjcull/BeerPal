using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Entities;
using BeerPal.Web.Models;
using BeerPal.Web.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BeerPal.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            MigrateAndSeedDatabase(host);

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static void MigrateAndSeedDatabase(IWebHost host)
        {
            // Migrate and seed the database during startup. Must be synchronous.
            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    scope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                    scope.ServiceProvider.GetService<SeedDatabaseService>().SeedDatabase().GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine("Failed to migrate or seed database: " + ex.Message);
            }
        }
    }
}
