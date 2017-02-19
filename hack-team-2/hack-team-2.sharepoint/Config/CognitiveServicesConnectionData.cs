using System.Configuration;

namespace hack_team_2.sharepoint.Config
{
    public class CognitiveServicesConnectionData
    {
        public static string SubscriptionKey => ConfigurationManager.AppSettings["OcrSubscriptionKey"];
    }
}
