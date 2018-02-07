using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Extensions;
using Microsoft.AspNetCore.Http;

namespace BeerPal.Web.Services
{
    public enum Gateway
    {
        [Display(Name = "paypal")]
        PayPal = 1,

        [Display(Name = "braintree")]
        BrainTree = 2,
        
        [Display(Name = "stripe")]
        Stripe = 3
    }

    public class GatewaySwitchService
    {
        private const string GatewaySession = "GATEWAY_SESSION";

        public Gateway CurrentGateway(ISession session)
        {
            var result = session.GetString(GatewaySession);

            var gateway = EnumHelper<Gateway>.GetValueFromName(result);

            if (gateway == default(Gateway))
            {
                gateway = Gateway.PayPal;
            }

            return gateway;
        }

        public string CurrentGatewayName(ISession session)
        {
            return CurrentGateway(session).ToString();
        }
        public string CurrentGatewayPath(ISession session)
        {
            return EnumHelper<Gateway>.GetDisplayValue(CurrentGateway(session));
        }

        public void SetCurrentGateway(ISession session, Gateway gateway)
        {
            session.SetString(GatewaySession, EnumHelper<Gateway>.GetDisplayValue(gateway));
        }
    }
}
