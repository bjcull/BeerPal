using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Data.Entities;
using BeerPal.Web.Models;
using BeerPal.Web.Services;
using BeerPal.Web.Settings;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BeerPal.Web
{
    public class Startup
    {
        private readonly int? _sslPort;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            if (Environment.IsDevelopment())
            {
                var launchConfiguration = new ConfigurationBuilder()
                    .SetBasePath(Environment.ContentRootPath)
                    .AddJsonFile(@"Properties\launchSettings.json")
                    .Build();
                // During development we won't be using port 443.
                _sslPort = launchConfiguration.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(options => Configuration.GetSection("AppSettings").Bind(options));

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(options => 
            { 
                options.Filters.Add(new RequireHttpsAttribute());
                options.SslPort = _sslPort;
            });

            // Add PayPal client factory.
            services.AddSingleton(factory => new PayPalHttpClientFactory(
                Configuration["PayPal:ClientId"], 
                Configuration["PayPal:ClientSecret"], 
                Convert.ToBoolean(Configuration["PayPal:IsLive"]))); // Is Live Environment?

            // Add Stripe client factory
            services.AddSingleton(factory => new StripeApiFactory(
                Configuration["Stripe:SecretKey"],
                Configuration["Stripe:PublishableKey"]));

            // Add Braintree client factory
            services.AddSingleton(factory => new BraintreeApiFactory(
                Configuration["Braintree:MerchantId"],
                Configuration["Braintree:PublicKey"],
                Configuration["Braintree:PrivateKey"]));

            services.AddTransient<SeedDatabaseService>();
            services.AddTransient<GatewaySwitchService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSession();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Cookies";
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = Configuration["AppSettings:BaseUrls:Auth"];
                    options.RequireHttpsMetadata = true;

                    options.ClientId = "beerpal_web";
                    options.ClientSecret = "=FBtmn&Nw+G3DVg@M&7jq%.+Y,oAcXz.XK>yBA+ozLv]ej9dcucPWFmmB?2YqQn3";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Clear();
                    options.Scope.Add("api1");
                    //options.Scope.Add("beerpal");
                    options.Scope.Add("offline_access");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area=paypal}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
