using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Braintree;
using Environment = Braintree.Environment;

namespace BeerPal.Web.Services
{
    public class BraintreeApiFactory
    {
        private readonly string _merchantId;
        private readonly string _publicKey;
        private readonly string _privateKey;

        public BraintreeApiFactory(string merchantId, string publicKey, string privateKey)
        {
            _merchantId = merchantId;
            _publicKey = publicKey;
            _privateKey = privateKey;
        }

        public BraintreeGateway GetClient()
        {
            return new BraintreeGateway()
            {
                MerchantId = _merchantId,
                PublicKey = _publicKey,
                PrivateKey = _privateKey,
                Environment = Environment.SANDBOX
            };
        }
    }
}
