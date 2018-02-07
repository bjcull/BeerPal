using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerPal.Web.ViewComponents
{
    public class GatewaySwitchModel
    {
        public GatewayModel CurrentGateway { get; set; }
        public List<GatewayModel> OtherGateways { get; set; } = new List<GatewayModel>();
    }
}
