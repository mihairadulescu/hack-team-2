using System.Collections.Generic;

namespace hack_team_2.Webhook.Models
{
    public class SPWebhookContent
    {
        public List<SPWebhookNotification> Value { get; set; }
    }
}