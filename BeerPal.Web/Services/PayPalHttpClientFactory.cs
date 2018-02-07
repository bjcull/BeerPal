using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPal.Core;

namespace BeerPal.Web.Services
{
    public class PayPalHttpClientFactory
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly bool _isLive;

        public PayPalHttpClientFactory(string clientId, string clientSecret, bool isLive)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _isLive = isLive;
        }

        public PayPalHttpClient GetClient()
        {
            if (_isLive)
            {
                // Live Environment
                return new PayPalHttpClient(new LiveEnvironment(_clientId, _clientSecret));
            }

            // Sandbox Environment
            return new PayPalHttpClient(new SandboxEnvironment(_clientId, _clientSecret));
        }
    }
}
