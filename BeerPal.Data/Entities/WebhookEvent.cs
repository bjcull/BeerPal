using System;

namespace BeerPal.Data.Entities
{
    public class WebhookEvent : BaseEntity
    {
        public string PayPalWebHookEventId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateReceived { get; set; }
        public string EventType { get; set; }
        public string Summary { get; set; }
        public string ResourceType { get; set; }
        public string ResourceJson { get; set; }        
    }
}