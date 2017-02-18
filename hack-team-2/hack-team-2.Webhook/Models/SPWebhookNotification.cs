// ReSharper disable InconsistentNaming
namespace hack_team_2.Webhook.Models
{
    public class SPWebhookNotification
    {
        public string SubscriptionId { get; set; }

        public string ClientState { get; set; }

        public string ExpirationDateTime { get; set; }

        public string Resource { get; set; }

        public string TenantId { get; set; }

        public string SiteUrl { get; set; }

        public string WebId { get; set; }
    }
}