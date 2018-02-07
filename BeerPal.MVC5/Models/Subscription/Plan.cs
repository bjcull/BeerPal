using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPal.Models.Subscription
{
    public class Plan
    {
        public string PayPalPlanId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfBeers { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }

        public static List<Plan> Plans => new List<Plan>()
        {
            new Plan()
            {
                Name = "Just Browsing Plan",
                Price = 500,
                PayPalPlanId = "P-27H34871BG666983A2CPYWUI", // Created in the BillingPlansController.
                NumberOfBeers = 1,
                Description1 = "Nothing else",
                Description2 = "Not even a thank you"
            },
            new Plan()
            {
                Name = "Let's Do This Plan",
                Price = 2495,
                PayPalPlanId = "P-6JH27250LV780153N2CPY5MA", // Created in the BillingPlansController.
                NumberOfBeers = 6,
                Description1 = "Welcome to the club",
                Description2 = "Thank you!"
            },
            new Plan()
            {
                Name = "Beard Included Plan",
                Price = 5995,
                PayPalPlanId = "P-94257379NN474020F2CPZDWY", // Created in the BillingPlansController.
                NumberOfBeers = 24,
                Description1 = "This plan used to be a secret",
                Description2 = "Not anymore I guess"
            },
            new Plan()
            {
                Name = "Hook It To My Veins Plan",
                Price = 10000,
                PayPalPlanId = "P-4H181978HP557725N2CPZKOI", // Created in the BillingPlansController.
                NumberOfBeers = 48,
                Description1 = "My personal plan",
                Description2 = "Should I just move in?"
            }
        };
    }


}