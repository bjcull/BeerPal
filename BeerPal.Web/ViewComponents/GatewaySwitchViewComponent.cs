using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeerPal.Web.Extensions;
using BeerPal.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeerPal.Web.ViewComponents
{
    public class GatewaySwitchViewComponent : ViewComponent
    {
        private readonly GatewaySwitchService _gatewaySwitchService;

        public GatewaySwitchViewComponent(GatewaySwitchService gatewaySwitchService)
        {
            _gatewaySwitchService = gatewaySwitchService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new GatewaySwitchModel()
            {
                CurrentGateway = new GatewayModel()
                    {
                DisplayName = _gatewaySwitchService.CurrentGatewayName(HttpContext.Session),
                PathName = _gatewaySwitchService.CurrentGatewayPath(HttpContext.Session)
                        }
            };

            model.OtherGateways = EnumHelper<Gateway>.GetDisplayValues()
                .Where(x => x != model.CurrentGateway.PathName)
                .Select(x => new GatewayModel()
                {
                    DisplayName = EnumHelper<Gateway>.GetValueFromName(x).ToString(),
                    PathName = x
                })
                .ToList();

            return View(model);
        }
    }
}
