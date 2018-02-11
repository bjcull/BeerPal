using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BeerPal.Auth.Settings;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;

namespace BeerPal.Auth.Resources
{
    public class BeerpalClientStore : IClientStore
    {
        private readonly AppSettings _appSettings;

        public BeerpalClientStore(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            if (clientId == "beerpal_web")
            {
                return GetWebClient();
            }

            throw new NotImplementedException();
        }

        private Client GetWebClient()
        {
            ///////////////////////////////////////////
            // BeerPal Web - Our client
            //////////////////////////////////////////
            return new Client
            {
                Enabled = true,
                ClientId = "beerpal_web",
                ClientName = "BeerPal Web",
                ClientUri = $"{_appSettings.BaseUrls.Web}",
                RequireConsent = false,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true,

                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Absolute,                
                AbsoluteRefreshTokenLifetime = 157700000, // 5 years. TODO: Consider better approach

                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,                
                ClientSecrets = new List<Secret>()
                {
                    new Secret("=FBtmn&Nw+G3DVg@M&7jq%.+Y,oAcXz.XK>yBA+ozLv]ej9dcucPWFmmB?2YqQn3".Sha256())
                },
                RedirectUris = new List<string>
                {
                    $"{_appSettings.BaseUrls.Web}signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    $"{_appSettings.BaseUrls.Web}signout-callback-oidc"
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "api1",
                    "beerpal",
                },
                AccessTokenLifetime = 3600
            };
        }
    }
}
