using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPal.Entities
{
    public class Subscription : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public string PayPalPlanId { get; set; }
        public string PayPalAgreementToken { get; set; }
        public string PayPalAgreementId { get; set; }
    }
}