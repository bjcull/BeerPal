using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace BeerPal.Auth.Resources
{
    public static class AuthResources
    {
        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api1", "Beerpal API", 
                    new List<string>() {})
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("beerpal", "BeerPal User Info",
                    new List<string>()
                    {
                        "brewery_name"
                    })
            };
        }
    }
}
