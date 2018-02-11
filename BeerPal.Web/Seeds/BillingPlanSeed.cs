using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Data.Entities;
using PayPal.BillingPlans;
using Stripe;

namespace BeerPal.Web.Seeds
{
    public static class BillingPlanSeed
    {
        public static List<BillingPlan> Plans => new List<BillingPlan>()
        {
            new BillingPlan()
            {
                Name = "Just Browsing Plan",
                Price = 500,
                NumberOfBeers = 1,
                Description1 = "Nothing else",
                Description2 = "Not even a thank you",
                BraintreePlanId = "just-browsing"
            },
            new BillingPlan()
            {
                Name = "Let's Do This Plan",
                Price = 2495,
                NumberOfBeers = 6,
                Description1 = "Welcome to the club",
                Description2 = "Thank you!",
                BraintreePlanId = "lets-do-this"
            },
            new BillingPlan()
            {
                Name = "Beard Included Plan",
                Price = 5995,
                NumberOfBeers = 24,
                Description1 = "This plan used to be a secret",
                Description2 = "Not anymore I guess",
                BraintreePlanId = "beard-included"
            },
            new BillingPlan()
            {
                Name = "Hook It To My Veins Plan",
                Price = 10000,
                NumberOfBeers = 48,
                Description1 = "My personal plan",
                Description2 = "Should I just move in?",
                BraintreePlanId = "hook-it-to-my-veins"
            }
        };

        public static List<Plan> PayPalPlans(string returnUrl, string cancelUrl) => new List<Plan>()
        {
            new Plan()
            {
                Name = "Just Browsing Plan",
                Description = "1 great new beer sent to your door each month.",
                Type = "infinite",
                State = "ACTIVE",
                PaymentDefinitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        Name = "Regular Payments",
                        Type = "REGULAR",
                        Frequency = "MONTH",
                        FrequencyInterval = "1",
                        Amount = new Currency()
                        {
                            CurrencyCode = "USD",
                            Value = "5.00"
                        },
                        Cycles = "0"
                    }
                },
                MerchantPreferences = new MerchantPreferences()
                {
                    // The initial payment
                    SetupFee = new Currency()
                    {
                        CurrencyCode = "USD",
                        Value = "5.00"
                    },
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                }
            },

            new Plan()
            {
                Name = "Let's Do This Plan",
                Description = "A refreshing 6-pack of assorted beers delivered to your door each month.",
                Type = "infinite",
                State = "ACTIVE",
                PaymentDefinitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        Name = "Regular Payments",
                        Type = "REGULAR",
                        Frequency = "MONTH",
                        FrequencyInterval = "1",
                        Amount = new Currency()
                        {
                            CurrencyCode = "USD",
                            Value = "24.95"
                        },
                        Cycles = "0"
                    }
                },
                MerchantPreferences = new MerchantPreferences()
                {
                    // The initial payment
                    SetupFee = new Currency()
                    {
                        CurrencyCode = "USD",
                        Value = "24.95"
                    },
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                }
            },

            new Plan()
            {
                Name = "Beard Included Plan",
                Description =
                    "A hand picked carton of the most delicious and rare beers placed delicately on your doorstep each month.",
                Type = "infinite",
                State = "ACTIVE",
                PaymentDefinitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        Name = "Regular Payments",
                        Type = "REGULAR",
                        Frequency = "MONTH",
                        FrequencyInterval = "1",
                        Amount = new Currency()
                        {
                            CurrencyCode = "USD",
                            Value = "59.95"
                        },
                        Cycles = "0"
                    }
                },
                MerchantPreferences = new MerchantPreferences()
                {
                    // The initial payment
                    SetupFee = new Currency()
                    {
                        CurrencyCode = "USD",
                        Value = "59.95"
                    },
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                }
            },

            new Plan()
            {
                Name = "Hook It To My Veins Plan",
                Description =
                    "Angels whisper sweet nothings into your ears as 48 precious glass bottles are carefully packed into your fridge each month.",
                Type = "infinite",
                State = "ACTIVE",
                PaymentDefinitions = new List<PaymentDefinition>()
                {
                    new PaymentDefinition()
                    {
                        Name = "Regular Payments",
                        Type = "REGULAR",
                        Frequency = "MONTH",
                        FrequencyInterval = "1",
                        Amount = new Currency()
                        {
                            CurrencyCode = "USD",
                            Value = "100.00"
                        },
                        Cycles = "0"
                    }
                },
                MerchantPreferences = new MerchantPreferences()
                {
                    // The initial payment
                    SetupFee = new Currency()
                    {
                        CurrencyCode = "USD",
                        Value = "100.00"
                    },
                    ReturnUrl = returnUrl,
                    CancelUrl = cancelUrl
                }
            }
        };

        public static List<StripePlanCreateOptions> StripePlans() => new List<StripePlanCreateOptions>()
        {
            new StripePlanCreateOptions()
            {
                Name = "Just Browsing Plan",
                Amount = 500,
                Currency = "USD",
                Interval = "month"
            },
            new StripePlanCreateOptions()
            {
                Name = "Let's Do This Plan",
                Amount = 2495,
                Currency = "USD",
                Interval = "month"
            },
            new StripePlanCreateOptions()
            {
                Name = "Beard Included Plan",
                Amount = 5995,
                Currency = "USD",
                Interval = "month"
            },
            new StripePlanCreateOptions()
            {
                Name = "Hook It To My Veins Plan",
                Amount = 10000,
                Currency = "USD",
                Interval = "month"
            },
        };
    }
}
