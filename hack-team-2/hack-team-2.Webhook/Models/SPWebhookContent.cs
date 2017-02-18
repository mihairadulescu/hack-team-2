using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace hack_team_2.Webhook.Models
{
    public class SPWebhookContent
    {
        public List<SPWebhookNotification> Value { get; set; }
    }
}